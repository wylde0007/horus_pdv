/**
 * Arquivo: API/NETCORE/Repositories/DatabaseAccess/HistoricoVendasAB.cs
 * Objetivo: concentra comandos SQL e persistência de histórico de vendas e recibos.
 * Entradas esperadas: recebe conexão configurada, parâmetros normalizados e executa leitura/escrita no SQL Server.
 */
using HORUSPDV_API.Models.Requests;
using HORUSPDV_API.Repositories.DataAccess;
using Npgsql;

namespace HORUSPDV_API.Repositories.DatabaseAccess;

public class HistoricoVendasAB(Connection connection)
{
    public async Task<List<VendaHistoricoAD>> ListarAsync(string companyId, string? saleNumber = null)
    {
        var hasSaleNumber = !string.IsNullOrWhiteSpace(saleNumber);

        var sql = """
            SELECT
                v.SaleNumber AS "SaleNumber",
                v.CustomerName AS "CustomerName",
                v.CustomerCpf AS "CustomerCpf",
                v.PaymentType AS "PaymentType",
                v.TotalAmount AS "TotalAmount",
                v.OperatorName AS "OperatorName",
                TO_CHAR(v.SaleDate AT TIME ZONE 'America/Sao_Paulo', 'DD/MM/YYYY HH24:MI:SS') AS "SaleDate",
                i.ProductCode AS "ProductCode",
                i.ProductName AS "ProductName",
                i.Quantity AS "Quantity",
                i.UnitPrice AS "UnitPrice",
                i.ItemTotal AS "ItemTotal"
            FROM VendaItens i
            INNER JOIN Vendas v ON v.Id = i.VendaId
            WHERE v.CompanyId = @CompanyId
            """;

        if (hasSaleNumber)
        {
            sql += """
                 AND v.SaleNumber = @SaleNumber
                """;
        }

        sql += """
            ORDER BY v.SaleDate DESC;
            """;

        await using var db = await connection.OpenConnectionAsync();
        await EnsurePrintColumnsAsync(db);
        await using var command = new NpgsqlCommand(sql, db);
        command.Parameters.AddWithValue("@CompanyId", companyId);

        if (hasSaleNumber)
        {
            command.Parameters.AddWithValue("@SaleNumber", saleNumber!.Trim());
        }

        await using var reader = await command.ExecuteReaderAsync();
        var rows = new List<VendaHistoricoAD>();

        while (await reader.ReadAsync())
        {
            rows.Add(Map(reader));
        }

        return rows;
    }


    public async Task<VendaRegistroResultadoAD> RegistrarAsync(string companyId, VendaRequest request)
    {
        await using var db = await connection.OpenConnectionAsync();
        await EnsurePrintColumnsAsync(db);
        await using var transaction = (NpgsqlTransaction)await db.BeginTransactionAsync();

        try
        {
            var saleNumber = await NextSaleNumberAsync(db, transaction);
            var now = DateTimeOffset.Now;
            var saleId = $"sale-{saleNumber}";
            var customerName = string.IsNullOrWhiteSpace(request.CustomerName) ? "Consumidor" : request.CustomerName.Trim();
            var customerCpf = string.IsNullOrWhiteSpace(request.CustomerCpf) ? "-" : request.CustomerCpf.Trim();
            var paymentType = string.IsNullOrWhiteSpace(request.PaymentType) ? "-" : request.PaymentType.Trim();
            var totalAmount = string.IsNullOrWhiteSpace(request.TotalAmount) ? "0,00" : request.TotalAmount.Trim();
            var operatorName = string.IsNullOrWhiteSpace(request.OperatorName) ? "Operador" : request.OperatorName.Trim();
            // A venda e a baixa de estoque compartilham a mesma transação para evitar histórico sem estoque atualizado.
            var saleItems = await BaixarEstoqueAsync(db, transaction, companyId, request.Items);

            await using (var saleCommand = new NpgsqlCommand(
                             """
                             INSERT INTO Vendas
                                 (Id, CompanyId, SaleNumber, CustomerName, CustomerCpf, PaymentType, TotalAmount, OperatorName, SaleDate)
                             VALUES
                                 (@Id, @CompanyId, @SaleNumber, @CustomerName, @CustomerCpf, @PaymentType, @TotalAmount, @OperatorName, @SaleDate);
                             """,
                             db,
                             transaction))
            {
                saleCommand.Parameters.AddWithValue("@Id", saleId);
                saleCommand.Parameters.AddWithValue("@CompanyId", companyId);
                saleCommand.Parameters.AddWithValue("@SaleNumber", saleNumber);
                saleCommand.Parameters.AddWithValue("@CustomerName", customerName);
                saleCommand.Parameters.AddWithValue("@CustomerCpf", customerCpf);
                saleCommand.Parameters.AddWithValue("@PaymentType", paymentType);
                saleCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                saleCommand.Parameters.AddWithValue("@OperatorName", operatorName);
                saleCommand.Parameters.AddWithValue("@SaleDate", now);
                await saleCommand.ExecuteNonQueryAsync();
            }

            var rows = new List<VendaHistoricoAD>();
            for (var index = 0; index < saleItems.Count; index += 1)
            {
                var item = saleItems[index];
                var itemId = $"{saleId}-item-{index + 1:000}";
                await using var itemCommand = new NpgsqlCommand(
                    """
                    INSERT INTO VendaItens
                        (Id, VendaId, ProductCode, ProductName, Quantity, UnitPrice, ItemTotal)
                    VALUES
                        (@Id, @VendaId, @ProductCode, @ProductName, @Quantity, @UnitPrice, @ItemTotal);
                    """,
                    db,
                    transaction);
                itemCommand.Parameters.AddWithValue("@Id", itemId);
                itemCommand.Parameters.AddWithValue("@VendaId", saleId);
                itemCommand.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                itemCommand.Parameters.AddWithValue("@ProductName", item.ProductName);
                itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                itemCommand.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                itemCommand.Parameters.AddWithValue("@ItemTotal", CalculateTotal(item.UnitPrice, item.Quantity));
                await itemCommand.ExecuteNonQueryAsync();

                rows.Add(new VendaHistoricoAD
                {
                    SaleNumber = saleNumber,
                    CustomerName = customerName,
                    CustomerCpf = customerCpf,
                    PaymentType = paymentType,
                    TotalAmount = totalAmount,
                    OperatorName = operatorName,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ItemTotal = CalculateTotal(item.UnitPrice, item.Quantity),
                    SaleDate = now.LocalDateTime.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }

            await transaction.CommitAsync();
            return new VendaRegistroResultadoAD { SaleNumber = saleNumber, Rows = rows };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task<string> NextSaleNumberAsync(NpgsqlConnection db, NpgsqlTransaction transaction)
    {
        await using (var lockCommand = new NpgsqlCommand(
            "SELECT pg_advisory_xact_lock(hashtext('vendas_sale_number'));",
            db,
            transaction))
        {
            await lockCommand.ExecuteNonQueryAsync();
        }

        await using var command = new NpgsqlCommand(
            """
            SELECT COALESCE(MAX(NULLIF(regexp_replace(SaleNumber, '\\D', '', 'g'), '')::integer), 15039) + 1
            FROM Vendas;
            """,
            db,
            transaction);

        return Convert.ToString(await command.ExecuteScalarAsync()) ?? "15040";
    }

    private static async Task<List<VendaItemRecord>> BaixarEstoqueAsync(
        NpgsqlConnection db,
        NpgsqlTransaction transaction,
        string companyId,
        IEnumerable<VendaItemRequest> items)
    {
        var groupedItems = items
            .GroupBy(item => item.ProductCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group => new VendaItemRecord
            {
                ProductCode = group.Key,
                ProductName = group.First().ProductName.Trim(),
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        foreach (var item in groupedItems)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantidade da venda deve ser maior que zero.");
            }

            await using var select = new NpgsqlCommand(
                """
                SELECT Id, ProductName, ProductQnt, ProductUnitPrice, ProductSalePrice
                FROM Produtos
                WHERE CompanyId = @CompanyId AND ProductCode = @ProductCode
                FOR UPDATE;
                """,
                db,
                transaction);
            select.Parameters.AddWithValue("@ProductCode", item.ProductCode);
            select.Parameters.AddWithValue("@CompanyId", companyId);
            await using var reader = await select.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException($"Produto {item.ProductCode} não encontrado.");
            }

            var productId = ReadString(reader, "Id");
            item.ProductName = ReadString(reader, "ProductName");
            var currentStock = ParseInt(ReadString(reader, "ProductQnt"));
            var unitPrice = ReadString(reader, "ProductUnitPrice");
            var salePrice = ReadString(reader, "ProductSalePrice");
            item.UnitPrice = string.IsNullOrWhiteSpace(salePrice) ? unitPrice : salePrice;
            await reader.CloseAsync();

            if (currentStock < item.Quantity)
            {
                throw new InvalidOperationException(
                    $"Estoque insuficiente para {item.ProductName}. Disponível: {currentStock}.");
            }

            var nextStock = currentStock - item.Quantity;
            await using var update = new NpgsqlCommand(
                """
                UPDATE Produtos
                   SET ProductQnt = @ProductQnt,
                       TotalPriceOnProduct = @TotalPriceOnProduct
                 WHERE Id = @Id;
                """,
                db,
                transaction);
            update.Parameters.AddWithValue("@ProductQnt", nextStock.ToString());
            update.Parameters.AddWithValue("@TotalPriceOnProduct", CalculateTotal(unitPrice, nextStock));
            update.Parameters.AddWithValue("@Id", productId);
            await update.ExecuteNonQueryAsync();
        }

        return groupedItems;
    }

    private static VendaHistoricoAD Map(NpgsqlDataReader reader) => new()
    {
        SaleNumber = ReadString(reader, "SaleNumber"),
        CustomerName = ReadString(reader, "CustomerName"),
        CustomerCpf = ReadString(reader, "CustomerCpf"),
        PaymentType = ReadString(reader, "PaymentType"),
        TotalAmount = ReadString(reader, "TotalAmount"),
        OperatorName = ReadString(reader, "OperatorName"),
        ProductCode = ReadString(reader, "ProductCode"),
        ProductName = ReadString(reader, "ProductName"),
        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
        UnitPrice = ReadString(reader, "UnitPrice"),
        ItemTotal = ReadString(reader, "ItemTotal"),
        SaleDate = ReadString(reader, "SaleDate")
    };

    private static async Task EnsurePrintColumnsAsync(NpgsqlConnection db)
    {
        const string schemaSql = """
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CustomerName VARCHAR(180) NOT NULL DEFAULT 'Consumidor';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CustomerCpf VARCHAR(30) NOT NULL DEFAULT '-';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS PaymentType VARCHAR(30) NOT NULL DEFAULT '-';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS TotalAmount VARCHAR(30) NOT NULL DEFAULT '0,00';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS OperatorName VARCHAR(180) NOT NULL DEFAULT 'Operador';
            ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS SaleDate TIMESTAMPTZ NOT NULL DEFAULT NOW();

            ALTER TABLE VendaItens ADD COLUMN IF NOT EXISTS UnitPrice VARCHAR(30) NOT NULL DEFAULT '0,00';
            ALTER TABLE VendaItens ADD COLUMN IF NOT EXISTS ItemTotal VARCHAR(30) NOT NULL DEFAULT '0,00';
            """;

        const string backfillSql = """
            UPDATE VendaItens i
               SET UnitPrice = COALESCE(NULLIF(TRIM(p.ProductSalePrice), ''), NULLIF(TRIM(p.ProductUnitPrice), ''), '0,00')
              FROM Produtos p
             WHERE p.ProductCode = i.ProductCode
               AND (
                    NULLIF(TRIM(i.UnitPrice), '') IS NULL
                    OR REPLACE(REPLACE(REPLACE(TRIM(i.UnitPrice), 'R$', ''), '.', ''), ',', '.') IN ('0', '0.00')
               );

            WITH normalized AS (
                SELECT
                    Id,
                    REGEXP_REPLACE(
                        REPLACE(REPLACE(REPLACE(TRIM(UnitPrice), 'R$', ''), '.', ''), ',', '.'),
                        '\s+',
                        '',
                        'g'
                    ) AS Amount
                FROM VendaItens
            )
            UPDATE VendaItens i
               SET ItemTotal = REPLACE(TO_CHAR((n.Amount::NUMERIC * i.Quantity), 'FM999999999990.00'), '.', ',')
              FROM normalized n
             WHERE n.Id = i.Id
               AND (
                    NULLIF(TRIM(i.ItemTotal), '') IS NULL
                    OR REPLACE(REPLACE(REPLACE(TRIM(i.ItemTotal), 'R$', ''), '.', ''), ',', '.') IN ('0', '0.00')
               )
               AND n.Amount ~ '^-?[0-9]+(\.[0-9]+)?$';
            """;

        await using (var schemaCommand = new NpgsqlCommand(schemaSql, db))
        {
            await schemaCommand.ExecuteNonQueryAsync();
        }

        await using var backfillCommand = new NpgsqlCommand(backfillSql, db);
        await backfillCommand.ExecuteNonQueryAsync();
    }


    private static int ParseInt(string value)
        => int.TryParse(value, out var parsed) ? parsed : 0;

    private static string CalculateTotal(string unitPrice, int quantity)
    {
        var normalized = unitPrice.Replace(".", "").Replace(",", ".");
        if (!decimal.TryParse(
                normalized,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out var parsed))
        {
            return "0,00";
        }

        return (parsed * quantity).ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
    }

    private static string ReadString(NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }

    private sealed class VendaItemRecord
    {
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string UnitPrice { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}

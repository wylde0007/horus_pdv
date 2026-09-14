/**
 * Arquivo: API/NETCORE/Repositories/DataAccess/ProdutoHistoricoAD.cs
 * Objetivo: representar uma alteração auditável de quantidade e preços de produto.
 */

namespace HORUSPDV_API.Repositories.DataAccess;

public class ProdutoHistoricoAD
{
    public string Id { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    public string ProductQntAntes { get; set; } = string.Empty;
    public string ProductQntDepois { get; set; } = string.Empty;

    public string ProductUnitPriceAntes { get; set; } = string.Empty;
    public string ProductUnitPriceDepois { get; set; } = string.Empty;

    public string ProductSalePriceAntes { get; set; } = string.Empty;
    public string ProductSalePriceDepois { get; set; } = string.Empty;

    public string ChangedFields { get; set; } = string.Empty;

    public string RequestId { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
}

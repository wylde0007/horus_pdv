/**
 * Arquivo: API/NETCORE/Repositories/DatabaseAccess/EmpresaAB.cs
 * Objetivo: concentra comandos SQL e persistência de dados cadastrais e configurações da empresa.
 * Entradas esperadas: recebe conexão configurada, parâmetros normalizados e executa leitura/escrita no SQL Server.
 */
using HORUSPDV_API.Repositories.DataAccess;
using HORUSPDV_API.Services.Security;
using Npgsql;

namespace HORUSPDV_API.Repositories.DatabaseAccess;

public class EmpresaAB(Connection connection, HorusSecretProtector secretProtector)
{
    public Task<EmpresaAD?> ObterPrincipalAsync()
        => ObterAsync("empresa-principal");

    public async Task<EmpresaAD?> ObterAsync(string companyId)
    {
        await using var db = await connection.OpenConnectionAsync();
        await using var command = new NpgsqlCommand(
            """
            SELECT FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone,
                   Phone, Mobile, Cep, Address, Number, Neighborhood, City, Uf, Complement,
                   EmailSmtpEnabled, EmailSmtpHost, EmailSmtpPort, EmailSmtpEnableSsl, EmailSmtpUser,
                   EmailSmtpPassword, EmailSmtpFromEmail, EmailSmtpFromName, EmailSmtpReplyTo
            FROM Empresas
            WHERE Id = @Id;
            """,
            db);
        command.Parameters.AddWithValue("@Id", companyId);
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public Task<EmpresaAD> SalvarPrincipalAsync(EmpresaAD empresa)
        => SalvarAsync("empresa-principal", empresa);

    public async Task<EmpresaAD> SalvarAsync(string companyId, EmpresaAD empresa)
    {
        await using var db = await connection.OpenConnectionAsync();
        await using var command = new NpgsqlCommand(
            """
            WITH updated AS (
            UPDATE Empresas
                   SET FantasyName = @FantasyName,
                       CorporateName = @CorporateName,
                       Cnpj = @Cnpj,
                       StateRegistration = @StateRegistration,
                       Website = @Website,
                       Email = @Email,
                       SacPhone = @SacPhone,
                       Phone = @Phone,
                       Mobile = @Mobile,
                       Cep = @Cep,
                       Address = @Address,
                       Number = @Number,
                       Neighborhood = @Neighborhood,
                       City = @City,
                       Uf = @Uf,
                       Complement = @Complement,
                       EmailSmtpEnabled = @EmailSmtpEnabled,
                       EmailSmtpHost = @EmailSmtpHost,
                       EmailSmtpPort = @EmailSmtpPort,
                       EmailSmtpEnableSsl = @EmailSmtpEnableSsl,
                       EmailSmtpUser = @EmailSmtpUser,
                       EmailSmtpPassword = CASE
                           WHEN @EmailSmtpPassword = '' THEN EmailSmtpPassword
                           ELSE @EmailSmtpPassword
                       END,
                       EmailSmtpFromEmail = @EmailSmtpFromEmail,
                       EmailSmtpFromName = @EmailSmtpFromName,
                       EmailSmtpReplyTo = @EmailSmtpReplyTo
                 WHERE Id = @Id
            RETURNING 1
            )
            INSERT INTO Empresas
                    (Id, FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone,
                     Phone, Mobile, Cep, Address, Number, Neighborhood, City, Uf, Complement,
                     EmailSmtpEnabled, EmailSmtpHost, EmailSmtpPort, EmailSmtpEnableSsl, EmailSmtpUser,
                     EmailSmtpPassword, EmailSmtpFromEmail, EmailSmtpFromName, EmailSmtpReplyTo)
            SELECT
            @Id, @FantasyName, @CorporateName, @Cnpj, @StateRegistration, @Website,
                     @Email, @SacPhone, @Phone, @Mobile, @Cep, @Address, @Number, @Neighborhood, @City, @Uf, @Complement,
                     @EmailSmtpEnabled, @EmailSmtpHost, @EmailSmtpPort, @EmailSmtpEnableSsl, @EmailSmtpUser,
                     @EmailSmtpPassword, @EmailSmtpFromEmail, @EmailSmtpFromName, @EmailSmtpReplyTo
            WHERE NOT EXISTS (SELECT 1 FROM updated);
            """,
            db);
        command.Parameters.AddWithValue("@Id", companyId);
        AddParameters(command, empresa);
        await command.ExecuteNonQueryAsync();
        return await ObterAsync(companyId) ?? empresa;
    }

    private void AddParameters(NpgsqlCommand command, EmpresaAD source)
    {
        command.Parameters.AddWithValue("@FantasyName", source.FantasyName.Trim());
        command.Parameters.AddWithValue("@CorporateName", source.CorporateName.Trim());
        command.Parameters.AddWithValue("@Cnpj", source.Cnpj.Trim());
        command.Parameters.AddWithValue("@StateRegistration", source.StateRegistration.Trim());
        command.Parameters.AddWithValue("@Website", source.Website.Trim());
        command.Parameters.AddWithValue("@Email", source.Email.Trim());
        command.Parameters.AddWithValue("@SacPhone", source.SacPhone.Trim());
        command.Parameters.AddWithValue("@Phone", source.Phone.Trim());
        command.Parameters.AddWithValue("@Mobile", source.Mobile.Trim());
        command.Parameters.AddWithValue("@Cep", source.Cep.Trim());
        command.Parameters.AddWithValue("@Address", source.Address.Trim());
        command.Parameters.AddWithValue("@Number", source.Number.Trim());
        command.Parameters.AddWithValue("@Neighborhood", source.Neighborhood.Trim());
        command.Parameters.AddWithValue("@City", source.City.Trim());
        command.Parameters.AddWithValue("@Uf", source.Uf.Trim());
        command.Parameters.AddWithValue("@Complement", source.Complement.Trim());
        command.Parameters.AddWithValue("@EmailSmtpEnabled", source.EmailSmtpEnabled);
        command.Parameters.AddWithValue("@EmailSmtpHost", source.EmailSmtpHost.Trim());
        command.Parameters.AddWithValue("@EmailSmtpPort", source.EmailSmtpPort);
        command.Parameters.AddWithValue("@EmailSmtpEnableSsl", source.EmailSmtpEnableSsl);
        command.Parameters.AddWithValue("@EmailSmtpUser", source.EmailSmtpUser.Trim());
        command.Parameters.AddWithValue(
            "@EmailSmtpPassword",
            string.IsNullOrWhiteSpace(source.EmailSmtpPassword)
                ? string.Empty
                : secretProtector.Protect(source.EmailSmtpPassword));
        command.Parameters.AddWithValue("@EmailSmtpFromEmail", source.EmailSmtpFromEmail.Trim());
        command.Parameters.AddWithValue("@EmailSmtpFromName", source.EmailSmtpFromName.Trim());
        command.Parameters.AddWithValue("@EmailSmtpReplyTo", source.EmailSmtpReplyTo.Trim());
    }

    private EmpresaAD Map(NpgsqlDataReader source) => new()
    {
        FantasyName = ReadString(source, "FantasyName"),
        CorporateName = ReadString(source, "CorporateName"),
        Cnpj = ReadString(source, "Cnpj"),
        StateRegistration = ReadString(source, "StateRegistration"),
        Website = ReadString(source, "Website"),
        Email = ReadString(source, "Email"),
        SacPhone = ReadString(source, "SacPhone"),
        Phone = ReadString(source, "Phone"),
        Mobile = ReadString(source, "Mobile"),
        Cep = ReadString(source, "Cep"),
        Address = ReadString(source, "Address"),
        Number = ReadString(source, "Number"),
        Neighborhood = ReadString(source, "Neighborhood"),
        City = ReadString(source, "City"),
        Uf = ReadString(source, "Uf"),
        Complement = ReadString(source, "Complement"),
        EmailSmtpEnabled = ReadBool(source, "EmailSmtpEnabled"),
        EmailSmtpHost = ReadString(source, "EmailSmtpHost"),
        EmailSmtpPort = ReadInt(source, "EmailSmtpPort"),
        EmailSmtpEnableSsl = ReadBool(source, "EmailSmtpEnableSsl"),
        EmailSmtpUser = ReadString(source, "EmailSmtpUser"),
        EmailSmtpPassword = UnprotectSecret(ReadString(source, "EmailSmtpPassword")),
        EmailSmtpFromEmail = ReadString(source, "EmailSmtpFromEmail"),
        EmailSmtpFromName = ReadString(source, "EmailSmtpFromName"),
        EmailSmtpReplyTo = ReadString(source, "EmailSmtpReplyTo")
    };

    private static string ReadString(NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }

    private static bool ReadBool(NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
    }

    private static int ReadInt(NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
    }

    private string UnprotectSecret(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        if (!value.StartsWith("enc:v1:", StringComparison.Ordinal)) return value;

        try
        {
            return secretProtector.Unprotect(value);
        }
        catch
        {
            return string.Empty;
        }
    }
}

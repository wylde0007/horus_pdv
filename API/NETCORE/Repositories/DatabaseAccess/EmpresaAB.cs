using HORUSPDV_API.Repositories.DataAccess;
using Microsoft.Data.SqlClient;

namespace HORUSPDV_API.Repositories.DatabaseAccess;

public class EmpresaAB(Connection connection)
{
    public async Task<EmpresaAD?> ObterPrincipalAsync()
    {
        await using var db = await connection.OpenConnectionAsync();
        await using var command = new SqlCommand(
            """
            SELECT FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone,
                   Phone, Mobile, Cep, Address, Number, Neighborhood, City, Uf, Complement
            FROM Empresas
            WHERE Id = N'empresa-principal';
            """,
            db);
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<EmpresaAD> SalvarPrincipalAsync(EmpresaAD empresa)
    {
        await using var db = await connection.OpenConnectionAsync();
        await using var command = new SqlCommand(
            """
            IF EXISTS (SELECT 1 FROM Empresas WHERE Id = N'empresa-principal')
            BEGIN
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
                       Complement = @Complement
                 WHERE Id = N'empresa-principal';
            END
            ELSE
            BEGIN
                INSERT INTO Empresas
                    (Id, FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone,
                     Phone, Mobile, Cep, Address, Number, Neighborhood, City, Uf, Complement)
                VALUES
                    (N'empresa-principal', @FantasyName, @CorporateName, @Cnpj, @StateRegistration, @Website,
                     @Email, @SacPhone, @Phone, @Mobile, @Cep, @Address, @Number, @Neighborhood, @City, @Uf, @Complement);
            END;
            """,
            db);
        AddParameters(command, empresa);
        await command.ExecuteNonQueryAsync();
        return empresa;
    }

    private static void AddParameters(SqlCommand command, EmpresaAD source)
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
    }

    private static EmpresaAD Map(SqlDataReader source) => new()
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
        Complement = ReadString(source, "Complement")
    };

    private static string ReadString(SqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }
}

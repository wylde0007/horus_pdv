using HORUSPDV_API.Models.Clientes;
using HORUSPDV_API.Models.Requests;
using HORUSPDV_API.Repositories.AcessoBanco;

namespace HORUSPDV_API.Services.Clientes;

public class ClienteService(HorusMockDatabase database) : IClienteService
{
    public Task<List<ClienteModel>> ListarAsync()
        => database.ListarClientesAsync();

    public async Task<ClienteModel> CriarAsync(ClienteRequest request)
    {
        Validate(request);
        var customer = MapRequest($"cl-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", request);
        return await database.SalvarClienteAsync(customer);
    }

    public async Task<ClienteModel?> AtualizarAsync(string id, ClienteRequest request)
    {
        Validate(request);
        var current = await database.ObterClienteAsync(id);
        if (current is null)
        {
            return null;
        }

        return await database.SalvarClienteAsync(MapRequest(id, request));
    }

    public Task<bool> ExcluirAsync(string id)
        => database.ExcluirClienteAsync(id);

    private static void Validate(ClienteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName) || request.CustomerName.Trim().Length < 3)
        {
            throw new InvalidOperationException("Nome do cliente deve ter no minimo 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Document))
        {
            throw new InvalidOperationException("Documento do cliente e obrigatorio.");
        }
    }

    private static ClienteModel MapRequest(string id, ClienteRequest request) => new()
    {
        Id = id,
        CustomerName = request.CustomerName.Trim(),
        Document = request.Document,
        BirthDate = request.BirthDate,
        Age = request.Age,
        Cep = request.Cep,
        City = request.City,
        State = request.State,
        Address = request.Address,
        Neighborhood = request.Neighborhood,
        StreetComplement = request.StreetComplement,
        Number = request.Number,
        ReferencePoint = request.ReferencePoint,
        Telephone = request.Telephone,
        Cellphone = request.Cellphone,
        Email = request.Email
    };
}

using HORUSPDV_API.Models.Fornecedores;
using HORUSPDV_API.Models.Requests;
using HORUSPDV_API.Repositories.AcessoBanco;

namespace HORUSPDV_API.Services.Fornecedores;

public class FornecedorService(HorusMockDatabase database) : IFornecedorService
{
    public Task<List<FornecedorModel>> ListarAsync()
        => database.ListarFornecedoresAsync();

    public async Task<FornecedorModel> CriarAsync(FornecedorRequest request)
    {
        Validate(request);
        var supplier = MapRequest($"fr-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", request);
        return await database.SalvarFornecedorAsync(supplier);
    }

    public async Task<FornecedorModel?> AtualizarAsync(string id, FornecedorRequest request)
    {
        Validate(request);
        var current = await database.ObterFornecedorAsync(id);
        if (current is null)
        {
            return null;
        }

        return await database.SalvarFornecedorAsync(MapRequest(id, request));
    }

    public Task<bool> ExcluirAsync(string id)
        => database.ExcluirFornecedorAsync(id);

    private static void Validate(FornecedorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName) || request.CompanyName.Trim().Length < 3)
        {
            throw new InvalidOperationException("Razao social deve ter no minimo 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.FantasyName) || request.FantasyName.Trim().Length < 3)
        {
            throw new InvalidOperationException("Nome fantasia deve ter no minimo 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Cnpj))
        {
            throw new InvalidOperationException("CNPJ do fornecedor e obrigatorio.");
        }
    }

    private static FornecedorModel MapRequest(string id, FornecedorRequest request) => new()
    {
        Id = id,
        CompanyName = request.CompanyName.Trim(),
        FantasyName = request.FantasyName.Trim(),
        Cnpj = request.Cnpj,
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

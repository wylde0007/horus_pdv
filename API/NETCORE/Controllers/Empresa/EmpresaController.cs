using HORUSPDV_API.Models.Requests;
using HORUSPDV_API.Models.Response;
using HORUSPDV_API.Repositories.DataAccess;
using HORUSPDV_API.Repositories.DatabaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Empresa;

[ApiController]
[Route("api/[controller]")]
public class EmpresaController(EmpresaAB empresaAB) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obter()
    {
        var empresa = await empresaAB.ObterPrincipalAsync();
        if (empresa is null)
        {
            return NotFound(new ApiResponse<EmpresaRequest> { Success = false, Message = "Empresa nao encontrada." });
        }

        return Ok(new ApiResponse<EmpresaRequest>
        {
            Success = true,
            Message = "Dados da empresa obtidos com sucesso.",
            Data = ToRequest(empresa)
        });
    }

    [HttpPut]
    public async Task<IActionResult> Atualizar([FromBody] EmpresaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FantasyName) || request.FantasyName.Trim().Length < 3)
        {
            return BadRequest(new ApiResponse<EmpresaRequest> { Success = false, Message = "Nome fantasia e obrigatorio." });
        }

        if (request.Cnpj.Count(char.IsDigit) != 14)
        {
            return BadRequest(new ApiResponse<EmpresaRequest> { Success = false, Message = "CNPJ invalido." });
        }

        var saved = await empresaAB.SalvarPrincipalAsync(ToDataAccess(request));
        return Ok(new ApiResponse<EmpresaRequest>
        {
            Success = true,
            Message = "Dados da empresa atualizados com sucesso.",
            Data = ToRequest(saved)
        });
    }

    private static EmpresaAD ToDataAccess(EmpresaRequest source) => new()
    {
        FantasyName = source.FantasyName,
        CorporateName = source.CorporateName,
        Cnpj = source.Cnpj,
        StateRegistration = source.StateRegistration,
        Website = source.Website,
        Email = source.Email,
        SacPhone = source.SacPhone,
        Phone = source.Phone,
        Mobile = source.Mobile,
        Cep = source.Cep,
        Address = source.Address,
        Number = source.Number,
        Neighborhood = source.Neighborhood,
        City = source.City,
        Uf = source.Uf,
        Complement = source.Complement
    };

    private static EmpresaRequest ToRequest(EmpresaAD source) => new()
    {
        FantasyName = source.FantasyName,
        CorporateName = source.CorporateName,
        Cnpj = source.Cnpj,
        StateRegistration = source.StateRegistration,
        Website = source.Website,
        Email = source.Email,
        SacPhone = source.SacPhone,
        Phone = source.Phone,
        Mobile = source.Mobile,
        Cep = source.Cep,
        Address = source.Address,
        Number = source.Number,
        Neighborhood = source.Neighborhood,
        City = source.City,
        Uf = source.Uf,
        Complement = source.Complement
    };
}

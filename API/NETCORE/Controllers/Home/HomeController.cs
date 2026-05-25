/**
 * Arquivo: API/NETCORE/Controllers/Home/HomeController.cs
 * Objetivo: expõe endpoints HTTP de indicadores e atalhos da página inicial e padroniza respostas para o frontend.
 * Entradas esperadas: recebe requisições REST, valida dados básicos e delega regras para serviços/repositórios.
 */
using HORUSPDV_API.Models.Response;
using HORUSPDV_API.Repositories.DatabaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Home;

[ApiController]
[Route("api/[controller]")]
public class HomeController(HomeAB homeAB) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obter()
        => Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Dados da home obtidos com sucesso.",
            Data = await homeAB.ObterAsync()
        });
}

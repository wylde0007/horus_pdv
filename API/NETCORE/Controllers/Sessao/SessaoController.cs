using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Sessao;

[ApiController]
[Route("api/[controller]")]
public class SessaoController : ControllerBase
{
    [HttpGet]
    public IActionResult Listar()
        => Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Sessoes obtidas com sucesso.",
            Data = new[]
            {
                new { id = "sess-1", device = "Mac - Firefox", location = "Localização indisponível", ip = "104.23.254.196", lastActive = "Agora mesmo", current = true, platform = "desktop" }
            }
        });
}

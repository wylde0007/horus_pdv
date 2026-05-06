using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Usuario;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    [HttpGet]
    public IActionResult Listar()
        => Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuarios obtidos com sucesso.",
            Data = new[]
            {
                new { id = "usr-001", cpf = "123.456.789-01", name = "Flávio Oliveira", email = "flavio@hpdv.com.br", phone = "(11) 98888-1111", role = "administrador", status = "ativo", createdAt = "2026-02-10", lastLoginAt = "2026-03-21T09:42:11", mustChangePassword = false },
                new { id = "usr-002", cpf = "234.567.890-12", name = "Maria Santos", email = "maria@hpdv.com.br", phone = "(11) 97777-2222", role = "gerente", status = "ativo", createdAt = "2026-02-15", lastLoginAt = "2026-03-20T18:05:33", mustChangePassword = false },
                new { id = "usr-003", cpf = "345.678.901-23", name = "João Costa", email = "joao@hpdv.com.br", phone = "(11) 96666-3333", role = "atendente", status = "inativo", createdAt = "2026-03-01", lastLoginAt = "2026-03-05T07:21:09", mustChangePassword = true }
            }
        });
}

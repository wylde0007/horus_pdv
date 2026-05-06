using HORUSPDV_API.Models.Requests;
using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Usuario;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private static readonly object SyncRoot = new();
    private static readonly List<UsuarioModel> Usuarios =
    [
        new() { Id = "usr-001", Cpf = "123.456.789-01", Name = "Flávio Oliveira", Email = "flavio@hpdv.com.br", Phone = "(11) 98888-1111", Role = "administrador", Status = "ativo", CreatedAt = "2026-02-10", LastLoginAt = "2026-03-21T09:42:11", MustChangePassword = false },
        new() { Id = "usr-002", Cpf = "234.567.890-12", Name = "Maria Santos", Email = "maria@hpdv.com.br", Phone = "(11) 97777-2222", Role = "gerente", Status = "ativo", CreatedAt = "2026-02-15", LastLoginAt = "2026-03-20T18:05:33", MustChangePassword = false },
        new() { Id = "usr-003", Cpf = "345.678.901-23", Name = "João Costa", Email = "joao@hpdv.com.br", Phone = "(11) 96666-3333", Role = "atendente", Status = "inativo", CreatedAt = "2026-03-01", LastLoginAt = "2026-03-05T07:21:09", MustChangePassword = true }
    ];

    [HttpGet]
    public IActionResult Listar()
        => Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuarios obtidos com sucesso.",
            Data = ListUsers()
        });

    [HttpPost]
    public IActionResult Criar([FromBody] UsuarioRequest request)
    {
        try
        {
            var user = MapRequest($"usr-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", request, true);
            lock (SyncRoot)
            {
                ValidateDuplicates(user, null);
                Usuarios.Insert(0, user);
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario criado com sucesso.",
                Data = CloneUser(user)
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object> { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(string id, [FromBody] UsuarioRequest request)
    {
        try
        {
            lock (SyncRoot)
            {
                var index = Usuarios.FindIndex(item => item.Id == id);
                if (index < 0)
                {
                    return NotFound(new ApiResponse<object> { Success = false, Message = "Usuario nao encontrado." });
                }

                var current = Usuarios[index];
                var updated = MapRequest(id, request, false);
                updated.CreatedAt = current.CreatedAt;
                updated.LastLoginAt = current.LastLoginAt;
                updated.MustChangePassword = !string.IsNullOrWhiteSpace(request.Password) || current.MustChangePassword;
                ValidateDuplicates(updated, id);
                Usuarios[index] = updated;
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Usuario atualizado com sucesso.",
                    Data = CloneUser(updated)
                });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object> { Success = false, Message = ex.Message });
        }
    }

    [HttpPatch("{id}/status")]
    public IActionResult AlterarStatus(string id, [FromBody] UsuarioStatusRequest request)
    {
        lock (SyncRoot)
        {
            var user = Usuarios.FirstOrDefault(item => item.Id == id);
            if (user is null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Usuario nao encontrado." });
            }

            user.Status = request.Status == "inativo" ? "inativo" : "ativo";
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Status atualizado com sucesso.",
                Data = CloneUser(user)
            });
        }
    }

    [HttpPost("{id}/resetar-senha")]
    public IActionResult ResetarSenha(string id)
    {
        lock (SyncRoot)
        {
            var user = Usuarios.FirstOrDefault(item => item.Id == id);
            if (user is null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Usuario nao encontrado." });
            }

            user.MustChangePassword = true;
            var password = $"Tmp@{Random.Shared.Next(100000, 999999)}9";
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Senha resetada com sucesso.",
                Data = new { user = CloneUser(user), password }
            });
        }
    }

    private static List<UsuarioModel> ListUsers()
    {
        lock (SyncRoot)
        {
            return Usuarios.Select(CloneUser).ToList();
        }
    }

    private static UsuarioModel MapRequest(string id, UsuarioRequest request, bool isCreate)
    {
        if (request.Cpf.Count(char.IsDigit) != 11) throw new InvalidOperationException("CPF invalido.");
        if (string.IsNullOrWhiteSpace(request.Name)) throw new InvalidOperationException("Nome e obrigatorio.");
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@')) throw new InvalidOperationException("E-mail invalido.");
        if (isCreate && request.Password.Length < 8) throw new InvalidOperationException("Senha deve ter no minimo 8 caracteres.");

        return new UsuarioModel
        {
            Id = id,
            Cpf = request.Cpf.Trim(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            Role = string.IsNullOrWhiteSpace(request.Role) ? "atendente" : request.Role.Trim(),
            Status = request.Status == "inativo" ? "inativo" : "ativo",
            CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            LastLoginAt = "-",
            MustChangePassword = isCreate || !string.IsNullOrWhiteSpace(request.Password)
        };
    }

    private static void ValidateDuplicates(UsuarioModel user, string? currentId)
    {
        if (Usuarios.Any(item => item.Id != currentId && OnlyDigits(item.Cpf) == OnlyDigits(user.Cpf)))
        {
            throw new InvalidOperationException("Ja existe usuario com este CPF.");
        }

        if (Usuarios.Any(item => item.Id != currentId && item.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Ja existe usuario com este e-mail.");
        }
    }

    private static string OnlyDigits(string value) => new(value.Where(char.IsDigit).ToArray());

    private static UsuarioModel CloneUser(UsuarioModel source) => new()
    {
        Id = source.Id,
        Cpf = source.Cpf,
        Name = source.Name,
        Email = source.Email,
        Phone = source.Phone,
        Role = source.Role,
        Status = source.Status,
        CreatedAt = source.CreatedAt,
        LastLoginAt = source.LastLoginAt,
        MustChangePassword = source.MustChangePassword
    };

    public class UsuarioStatusRequest
    {
        public string Status { get; set; } = "ativo";
    }

    private class UsuarioModel
    {
        public string Id { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string LastLoginAt { get; set; } = string.Empty;
        public bool MustChangePassword { get; set; }
    }
}

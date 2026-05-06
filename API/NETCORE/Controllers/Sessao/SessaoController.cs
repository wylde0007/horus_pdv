using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.Sessao;

[ApiController]
[Route("api/[controller]")]
public class SessaoController : ControllerBase
{
    private static readonly object SyncRoot = new();
    private static readonly List<SessaoModel> Sessoes =
    [
        new() { Id = "sess-1", Device = "Mac - Firefox", Location = "Localização indisponível", Ip = "104.23.254.196", LastActive = "Agora mesmo", Current = true, Platform = "desktop" },
        new() { Id = "sess-2", Device = "iPhone - Safari", Location = "São Paulo, SP", Ip = "189.10.44.12", LastActive = "Há 18 minutos", Current = false, Platform = "mobile" },
        new() { Id = "sess-3", Device = "Windows - Chrome", Location = "Osasco, SP", Ip = "177.33.10.90", LastActive = "Hoje às 08:12", Current = false, Platform = "desktop" }
    ];

    [HttpGet]
    public IActionResult Listar()
    {
        lock (SyncRoot)
        {
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Sessoes obtidas com sucesso.",
                Data = Sessoes.Select(CloneSession).ToList()
            });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Encerrar(string id)
    {
        lock (SyncRoot)
        {
            var session = Sessoes.FirstOrDefault(item => item.Id == id);
            if (session is null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Sessao nao encontrada." });
            }

            if (session.Current)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "A sessao atual nao pode ser encerrada por esta acao." });
            }

            Sessoes.Remove(session);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Sessao encerrada com sucesso.",
                Data = Sessoes.Select(CloneSession).ToList()
            });
        }
    }

    [HttpDelete("outras")]
    public IActionResult EncerrarOutras()
    {
        lock (SyncRoot)
        {
            Sessoes.RemoveAll(item => !item.Current);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Outras sessoes encerradas com sucesso.",
                Data = Sessoes.Select(CloneSession).ToList()
            });
        }
    }

    private static SessaoModel CloneSession(SessaoModel source) => new()
    {
        Id = source.Id,
        Device = source.Device,
        Location = source.Location,
        Ip = source.Ip,
        LastActive = source.LastActive,
        Current = source.Current,
        Platform = source.Platform
    };

    private class SessaoModel
    {
        public string Id { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string LastActive { get; set; } = string.Empty;
        public bool Current { get; set; }
        public string Platform { get; set; } = "desktop";
    }
}

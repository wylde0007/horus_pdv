using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.ModuloMercado;

[ApiController]
[Route("api/[controller]")]
public class ModuloMercadoController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Obter(string id)
    {
        var config = BuildConfig(id);
        if (config is null)
        {
            return NotFound(new ApiResponse<object> { Success = false, Message = "Modulo nao encontrado." });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Modulo obtido com sucesso.",
            Data = config
        });
    }

    private static object? BuildConfig(string id)
    {
        var title = id switch
        {
            "fiscal" => "Fiscal NFC-e / NF-e",
            "pagamentos" => "Pagamentos Integrados",
            "estoque" => "Estoque e Inventário",
            "caixa" => "Abertura e Fechamento de Caixa",
            "compras" => "Compras e Reposição",
            "devolucoes" => "Trocas e Devoluções",
            "crm-fidelidade" => "CRM e Fidelidade",
            "omnichannel" => "Omnichannel e Integrações",
            _ => null
        };

        if (title is null) return null;

        return new
        {
            title,
            description = $"Dados temporários da API para o módulo {title}.",
            primaryAction = id switch
            {
                "fiscal" => "Nova NFC-e",
                "pagamentos" => "Nova cobrança",
                "estoque" => "Ajustar estoque",
                "caixa" => "Abrir caixa",
                "compras" => "Novo pedido",
                "devolucoes" => "Nova devolução",
                "crm-fidelidade" => "Nova campanha",
                "omnichannel" => "Conectar canal",
                _ => "Nova ação"
            },
            statusLabel = "Status do módulo",
            statusValue = "Operação em homologação",
            kpis = new[]
            {
                new { label = "Volume", value = "128", hint = "Dados mantidos na API", tone = "secondary" },
                new { label = "Pendências", value = "3", hint = "Aguardando ação", tone = "accent" },
                new { label = "Concluídos", value = "92%", hint = "Fluxos simulados", tone = "success" },
                new { label = "Alertas", value = "5", hint = "Pontos de atenção", tone = "primary" }
            },
            recordsTitle = "Registros operacionais",
            records = new[]
            {
                new { id = $"{id}-001", title = $"{title} - Registro 001", description = "Registro principal vindo da API.", status = "Ativo", amount = "R$ 184,90", meta = "Sincronizado agora" },
                new { id = $"{id}-002", title = $"{title} - Registro 002", description = "Item aguardando validação operacional.", status = "Pendente", amount = "R$ 59,80", meta = "Prioridade média" },
                new { id = $"{id}-003", title = $"{title} - Registro 003", description = "Evento de auditoria e acompanhamento.", status = "Auditado", amount = "R$ 1.240,00", meta = "Responsável: Administrador" }
            },
            workflowTitle = "Fluxo operacional",
            workflow = new[]
            {
                "Carregar dados do módulo pela API.",
                "Validar informações e regras temporárias.",
                "Executar ação operacional no frontend.",
                "Persistir futuramente no banco definitivo."
            },
            alerts = new[]
            {
                "Dados ainda residem em memória na API.",
                "Endpoint preparado para futura persistência em banco.",
                "Fluxo conectado ao frontend sem dependência de mock local."
            }
        };
    }
}

using HORUSPDV_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HORUSPDV_API.Controllers.HistoricoVendas;

[ApiController]
[Route("api/[controller]")]
public class HistoricoVendasController : ControllerBase
{
    [HttpGet]
    public IActionResult Listar()
        => Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Historico de vendas obtido com sucesso.",
            Data = new[]
            {
                new { saleNumber = "15039", customerName = "Ana Martins", customerCpf = "123.456.789-09", productCode = "CAF500", productName = "Café Tradicional 500g", quantity = 3, saleDate = "21/03/2026 14:12:08" },
                new { saleNumber = "15038", customerName = "Lucas Souza", customerCpf = "427.632.180-01", productCode = "ACH400", productName = "Achocolatado 400g", quantity = 1, saleDate = "21/03/2026 13:42:11" },
                new { saleNumber = "15037", customerName = "Beatriz Lima", customerCpf = "064.822.390-16", productCode = "ARR5KG", productName = "Arroz Tipo 1 5kg", quantity = 2, saleDate = "21/03/2026 12:55:46" }
            }
        });
}

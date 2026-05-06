namespace HORUSPDV_API.Repositories.DataAccess;

public class CaixaSessionAD
{
    public string Id { get; set; } = string.Empty;
    public DateTimeOffset OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public string OpeningAmount { get; set; } = "0,00";
    public string ClosingAmount { get; set; } = "0,00";
    public string OperatorName { get; set; } = string.Empty;
    public string ClosedByName { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}

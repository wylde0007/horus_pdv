namespace HORUSPDV_API.Services.Email;

public class HorusEmailOptions
{
    public bool Enabled { get; set; }
    public string Host { get; set; } = "smtp.office365.com";
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string User { get; set; } = "naoresponderhoruspdv@outlook.com";
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = "naoresponderhoruspdv@outlook.com";
    public string FromName { get; set; } = "Hórus PDV";
    public string ReplyTo { get; set; } = string.Empty;
    public string FrontendBaseUrl { get; set; } = "http://localhost:5173";
}

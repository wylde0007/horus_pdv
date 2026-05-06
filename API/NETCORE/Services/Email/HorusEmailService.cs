using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace HORUSPDV_API.Services.Email;

public class HorusEmailService(
    IOptions<HorusEmailOptions> options,
    ILogger<HorusEmailService> logger)
{
    private readonly HorusEmailOptions _options = options.Value;

    public bool IsEnabled => _options.Enabled;

    public async Task SendPasswordResetEmailAsync(
        string toEmail,
        string companyName,
        string resetUrl,
        DateTimeOffset expiresAt,
        CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("Envio de e-mail desabilitado. Link de recuperação gerado para {Email}.", toEmail);
            return;
        }

        ValidateConfiguration();
        if (string.IsNullOrWhiteSpace(toEmail) || !toEmail.Contains('@'))
        {
            throw new InvalidOperationException("E-mail de destino inválido para recuperação de senha.");
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_options.FromEmail, _options.FromName),
            Subject = "Recuperação de senha - Hórus PDV",
            Body = HorusEmailTemplate.BuildPasswordResetHtml(companyName, resetUrl, expiresAt),
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(toEmail.Trim().ToLowerInvariant()));
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
            HorusEmailTemplate.BuildPasswordResetText(companyName, resetUrl, expiresAt),
            null,
            "text/plain"));

        if (!string.IsNullOrWhiteSpace(_options.ReplyTo))
        {
            message.ReplyToList.Add(new MailAddress(_options.ReplyTo));
        }

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            Credentials = new NetworkCredential(_options.User, _options.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        await client.SendMailAsync(message, cancellationToken);
    }

    public async Task SendSignupWelcomeEmailAsync(
        string toEmail,
        string companyName,
        CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("Envio de e-mail desabilitado. E-mail de cadastro nao enviado para {Email}.", toEmail);
            return;
        }

        ValidateConfiguration();
        if (string.IsNullOrWhiteSpace(toEmail) || !toEmail.Contains('@'))
        {
            throw new InvalidOperationException("E-mail de destino inválido para cadastro.");
        }

        var loginUrl = ResolveFrontendBaseUrl();
        using var message = new MailMessage
        {
            From = new MailAddress(_options.FromEmail, _options.FromName),
            Subject = "Cadastro criado - Hórus PDV",
            Body = HorusEmailTemplate.BuildSignupWelcomeHtml(companyName, loginUrl),
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(toEmail.Trim().ToLowerInvariant()));
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
            HorusEmailTemplate.BuildSignupWelcomeText(companyName, loginUrl),
            null,
            "text/plain"));

        if (!string.IsNullOrWhiteSpace(_options.ReplyTo))
        {
            message.ReplyToList.Add(new MailAddress(_options.ReplyTo));
        }

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            Credentials = new NetworkCredential(_options.User, _options.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        await client.SendMailAsync(message, cancellationToken);
    }

    public string BuildPasswordResetUrl(string token)
    {
        var baseUrl = ResolveFrontendBaseUrl();
        var encodedToken = Uri.EscapeDataString(token.Trim());
        return $"{baseUrl}/?resetToken={encodedToken}";
    }

    private string ResolveFrontendBaseUrl()
        => string.IsNullOrWhiteSpace(_options.FrontendBaseUrl)
            ? "http://localhost:5173"
            : _options.FrontendBaseUrl.Trim().TrimEnd('/');

    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_options.Host) ||
            _options.Port <= 0 ||
            string.IsNullOrWhiteSpace(_options.User) ||
            string.IsNullOrWhiteSpace(_options.Password) ||
            string.IsNullOrWhiteSpace(_options.FromEmail))
        {
            throw new InvalidOperationException("Serviço de e-mail não configurado. Configure Email:Password e Email:Enabled.");
        }
    }
}

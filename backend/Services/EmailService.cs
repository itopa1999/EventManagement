using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using backend.Dtos;

public class EmailService
{
    private readonly SmtpSettingsDto _smtpSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettingsDto> smtpSettings,ILogger<EmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your Name", _smtpSettings.FromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };
        Console.WriteLine($"From: {message.From}");
        Console.WriteLine($"To: {message.To}");
        Console.WriteLine($"Subject: {message.Subject}");
        Console.WriteLine($"Body: {message.Body}");

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.FromEmail, _smtpSettings.FromPassword);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}

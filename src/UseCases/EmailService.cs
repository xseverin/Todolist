namespace UseCases;

using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPass)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _smtpUser = smtpUser;
        _smtpPass = smtpPass;
    }

    public void SendEmail(string fromEmail, string fromName, string toEmail, string toName, string subject, string body)
    {
        using (var client = new SmtpClient())
        {
            client.Host = _smtpHost;
            client.Port = _smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

            using (var message = new MailMessage(
                       from: new MailAddress(fromEmail, fromName),
                       to: new MailAddress(toEmail, toName)
                   ))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                client.Send(message);
            }
        }
    }
}
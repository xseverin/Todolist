namespace UseCases;

public interface IEmailService
{
    void SendEmail(string from, string fromName, string to, string toName, string subject, string body);
}
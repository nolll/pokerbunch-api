using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Messages;

namespace Infrastructure.Email;

public class SmtpEmailSender(string smtpHost, int port, string? userName = null, string? password = null) : EmailSender
{
    public override Task SendAsync(string to, IMessage message) => 
        Client.SendMailAsync(new MailMessage($"{FromName} <{FromEmail}>", to, message.Subject, message.Body));

    private SmtpClient Client => new(smtpHost, port)
    {
        Credentials = Credentials
    };

    private NetworkCredential? Credentials => userName != null && password != null
        ? new NetworkCredential(userName, password)
        : null;
}
using System.Net;
using System.Net.Mail;
using Core;

namespace Infrastructure.Email;

public class SmtpEmailSender(string smtpHost, int port, string? userName = null, string? password = null) : EmailSender
{
    public override void Send(string to, IMessage message) => 
        Client.Send(new MailMessage($"{FromName} <{FromEmail}>", to, message.Subject, message.Body));

    private SmtpClient Client => new(smtpHost, port)
    {
        Credentials = userName != null && password != null
            ? new NetworkCredential(userName, password)
            : null
    };
}
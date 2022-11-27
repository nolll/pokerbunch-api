using System.Net;
using System.Net.Mail;
using Core;

namespace Infrastructure.Email;

public class SmtpEmailSender : EmailSender
{
    private readonly string _smtpHost;
    private readonly int _port;
    private readonly string _userName;
    private readonly string _password;

    public SmtpEmailSender(string smtpHost, int port, string userName = null, string password = null)
    {
        _smtpHost = smtpHost;
        _port = port;
        _userName = userName;
        _password = password;
    }

    public override void Send(string to, IMessage message)
    {
        const string from = $"{FromName} <{FromEmail}>";
        var email = new MailMessage(from, to, message.Subject, message.Body);

        Client.Send(email);
    }

    private SmtpClient Client
    {
        get
        {
            var client =  new SmtpClient(_smtpHost);
            client.Credentials = _userName != null && _password != null
                ? new NetworkCredential(_userName, _password)
                : null;
            client.Port = _port;
            return client;
        }
    }
}
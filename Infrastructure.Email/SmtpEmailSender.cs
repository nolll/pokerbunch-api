using System.Net.Mail;
using Core;

namespace Infrastructure.Email
{
    public class SmtpEmailSender : EmailSender
    {
	    private readonly string _smtpHost;

	    public SmtpEmailSender(string smtpHost)
	    {
	        _smtpHost = smtpHost;
	    }

	    public override void Send(string to, IMessage message)
	    {
	        var from = $"{FromName} <{FromEmail}>";
            var email = new MailMessage(from, to, message.Subject, message.Body);

            Client.Send(email);
        }

	    private SmtpClient Client => new SmtpClient(_smtpHost);
    }
}
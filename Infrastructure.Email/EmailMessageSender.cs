using System.Net;
using System.Net.Mail;
using Core;
using Core.Services;

namespace Infrastructure.Email
{
	public class EmailMessageSender : IMessageSender
    {
	    private readonly string _smtpHost;
	    private readonly string _smtpUserName;
	    private readonly string _smtpPassword;

	    public EmailMessageSender(string smtpHost, string smtpUserName, string smtpPassword)
	    {
	        _smtpHost = smtpHost;
	        _smtpUserName = smtpUserName;
	        _smtpPassword = smtpPassword;
	    }

	    public void Send(string to, IMessage message)
        {
            const string from = "PokerBunch.com <noreply@pokerbunch.com>";

            var email = new MailMessage(from, to, message.Subject, message.Body);

            Client.Send(email);
        }

	    private SmtpClient Client
	    {
	        get
	        {
                var client = new SmtpClient(_smtpHost);
                if(HasCredentials)
                    client.Credentials = Credentials;
	            return client;
	        }
	    }

        private NetworkCredential Credentials => HasCredentials ? null : new NetworkCredential(_smtpUserName, _smtpPassword);
        private bool HasCredentials => !string.IsNullOrEmpty(_smtpUserName) && !string.IsNullOrEmpty(_smtpPassword);
    }
}
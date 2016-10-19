using System.Net.Mail;
using Core;
using Core.Services;

namespace Infrastructure.Email
{
	public class EmailMessageSender : IMessageSender
    {
        public void Send(string to, IMessage message)
        {
            const string from = "PokerBunch.com <noreply@pokerbunch.com>";

            var email = new MailMessage(from, to, message.Subject, message.Body);

            var client = new SmtpClient();
            client.Send(email);
        }
	}
}
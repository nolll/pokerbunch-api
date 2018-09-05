using Core;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email
{
    public class SendGridEmailSender : EmailSender
    {
        private readonly string _apiKey;

        public SendGridEmailSender(string apiKey)
        {
            _apiKey = apiKey;
        }

        public override void Send(string to, IMessage message)
        {
            var client = new SendGridClient(_apiKey);
            var fromAddress = new EmailAddress(FromEmail, FromName);
            var toAddress = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromAddress, toAddress, message.Subject, message.Body, message.Body);
            var response = client.SendEmailAsync(msg).Result;
        }
    }
}
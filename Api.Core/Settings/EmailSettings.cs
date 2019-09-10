namespace Api.Settings
{
    public class EmailSettings
    {
        public EmailProvider Provider { get; set; }
        public EmailSmtpSettings Smtp {get; set; }
        public EmailSendGridSettings SendGrid { get; set; }
    }
}
using JetBrains.Annotations;

namespace Api
{
    [UsedImplicitly]
    public class AppSettings
    {
        public string SiteHost { get; [UsedImplicitly] set; }
        public string ApiHost { get; [UsedImplicitly] set; }
        public string SqlConnectionString { get; [UsedImplicitly] set; }
        public string EmailSender { get; [UsedImplicitly] set; }
        public string SmtpHost { get; [UsedImplicitly] set; }
        public string SendGridApiKey { get; [UsedImplicitly] set; }
        public bool AllowAuthOverride { get; [UsedImplicitly] set; }
        public string NoAuthAdminUserName { get; [UsedImplicitly] set; }
        public string NoAuthPlayerUserName { get; [UsedImplicitly] set; }
        public string AuthSecret { get; [UsedImplicitly] set; }
    }
}
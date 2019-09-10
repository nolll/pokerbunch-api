using System.Configuration;

namespace Api
{
    public class Settings
    {
        private readonly AppSettings _appSettings;
        public string SiteHost => _appSettings.SiteHost ?? "pokerbunch.com";
        public string ApiHost => _appSettings.ApiHost ?? "api.pokerbunch.com";
        public string ConnectionString => _appSettings.SqlConnectionString;
        public bool UseSendGrid => _appSettings.EmailSender == "SendGrid";
        public string SmtpHost => _appSettings.SmtpHost;
        public string SendGridApiKey => _appSettings.SendGridApiKey;
        public bool AllowAuthOverride => _appSettings.AllowAuthOverride;
        public string NoAuthAdminUserName => _appSettings.NoAuthAdminUserName;
        public string NoAuthPlayerUserName => _appSettings.NoAuthPlayerUserName;
        public string AuthSecret => _appSettings.AuthSecret;
        public string Version => _appSettings.Version;

        public Settings(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
    }
}

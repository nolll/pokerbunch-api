using System.Configuration;

namespace Api
{
    public class Settings
    {
        public static string SiteHost => Get("SiteHost") ?? "pokerbunch.com";
        public static string ApiHost => Get("ApiHost") ?? "api.pokerbunch.com";
        public static string ConnectionString => Get("SqlConnectionString");
        public static bool UseSendGrid => Get("EmailSender") == "SendGrid";
        public static string SmtpHost => Get("SmtpHost");
        public static string SendGridApiKey => Get("SendGridApiKey");
        public static bool AllowAuthOverride => GetBool("AllowAuthOverride");
        public static string NoAuthAdminUserName => Get("NoAuthAdminUserName");
        public static string NoAuthPlayerUserName => Get("NoAuthPlayerUserName");

        private static bool GetBool(string key)
        {
            bool ret;
            var str = Get(key);
            return bool.TryParse(str, out ret) ? ret : ret;
        }

        private static string Get(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}

using System.Configuration;

namespace Api
{
    public class Settings
    {
        public static string SiteHost => Get("SiteHost");
        public static string ApiHost => Get("ApiHost");
        public static string ConnectionString => Get("SqlConnectionString");
        public static string SmtpHost => Get("SmtpHost");
        public static string SmtpUserName => Get("SmtpUserName");
        public static string SmtpPassword => Get("SmtpPassword");
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

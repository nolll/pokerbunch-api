namespace Api
{
    public class ApiSettings : CommonSettings
    {
        public static string SiteHost => Get("SiteHost");
        public static string ApiHost => Get("ApiHost");
        public static string ConnectionString => Get("SqlConnectionString");
        public static bool AllowAuthOverride => GetBool("AllowAuthOverride");
        public static string NoAuthAdminUserName => Get("NoAuthAdminUserName");
        public static string NoAuthPlayerUserName => Get("NoAuthPlayerUserName");
    }
}

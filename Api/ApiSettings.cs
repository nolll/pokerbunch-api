using Web.Common;

namespace Api
{
    public class ApiSettings : CommonSettings
    {
        public static string SiteHost => Get("SiteHost");
        public static string ApiHost => Get("ApiHost");
        public static bool RequireAuthorization => GetBool("RequireAuthorization");
        public static string ConnectionString => GetConnectionString("pokerbunch");
    }
}

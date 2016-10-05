using System.Configuration;

namespace Web.Common
{
    public abstract class CommonSettings
    {
        protected static bool GetBool(string key)
        {
            bool ret;
            var str = Get(key);
            return bool.TryParse(str, out ret) ? ret : ret;
        }

        protected static string Get(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
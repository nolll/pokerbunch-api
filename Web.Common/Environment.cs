namespace Web.Common
{
    public static class Environment
    {
        public static bool IsNoAuth(string hostName)
        {
            return hostName.Contains("no-auth");
        }

        public static bool IsDev(string hostName)
        {
            return hostName.EndsWith("pokerbunch.lan");
        }

        public static bool IsTest(string hostName)
        {
            return hostName.EndsWith("homeip.net");
        }

        public static bool IsStage(string hostName)
        {
            return hostName.EndsWith("staging.pokerbunch.com");
        }
    }
}
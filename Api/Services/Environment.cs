namespace Api.Services
{
    public static class Environment
    {
        public static bool IsDevMode(string hostName)
        {
            return IsDevModeAdmin(hostName) || IsDevModePlayer(hostName);
        }

        public static bool IsDevModeAdmin(string hostName)
        {
            return IsLocal(hostName) && hostName.Contains("api-admin");
        }

        public static bool IsDevModePlayer(string hostName)
        {
            return IsLocal(hostName) && hostName.Contains("api-player");
        }

        private static bool IsLocal(string hostName)
        {
            return hostName.EndsWith(".lan");
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
namespace Api.Services
{
    public class Environment
    {
        private readonly string _hostName;

        public Environment(string hostName)
        {
            _hostName = hostName;
        }

        public bool IsDevMode => IsDevModeAdmin || IsDevModePlayer;
        public bool IsDevModeAdmin => IsLocal && Contains("api-admin");
        public bool IsDevModePlayer => IsLocal && Contains("api-player");
        public bool IsDev => EndsWith("pokerbunch.local");
        public bool IsTest => EndsWith("homeip.net");
        public bool IsStage => EndsWith("staging.pokerbunch.com");
        public bool IsAnyTest => IsDev || IsTest || IsStage;
        private bool IsLocal => EndsWith(".local");
        private bool EndsWith(string s) => _hostName.EndsWith(s);
        private bool Contains(string s) => _hostName.Contains(s);
    }
}
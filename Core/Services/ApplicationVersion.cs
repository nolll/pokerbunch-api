using System;
using System.Linq;

namespace Core.Services
{
    public class ApplicationVersion
    {
        public string DisplayVersion { get; }

        public ApplicationVersion()
        {
            var fullVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            DisplayVersion = FormatDisplayFormat(fullVersion);
        }

        private string FormatDisplayFormat(string fullVersion)
        {
            if (NeedsShortening(fullVersion))
                return Shorten(fullVersion);
            return fullVersion;
        }

        private string Shorten(string fullVersion)
        {
            return fullVersion.Substring(0, fullVersion.LastIndexOf(".", StringComparison.Ordinal));
        }

        private bool NeedsShortening(string fullVersion)
        {
            return GetNumberOfDots(fullVersion) > 2;
        }

        private int GetNumberOfDots(string fullVersion)
        {
            return fullVersion.Count(s => s == '.');
        }
    }
}
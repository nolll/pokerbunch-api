using System.Collections.Generic;

namespace Api.Middlewares.Headers
{
    public class SecurityHeadersPolicy
    {
        public IDictionary<string, string> SetHeaders { get; }
            = new Dictionary<string, string>();

        public ISet<string> RemoveHeaders { get; }
            = new HashSet<string>();
    }
}
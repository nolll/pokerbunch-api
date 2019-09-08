using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Api.Models.HomeModels
{
    [DataContract(Namespace = "", Name = "application")]
    public class VersionModel
    {
        [DataMember(Name = "version")]
        public string VersionNumber
        {
            get
            {
                var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (assemblyVersion.Count(x => x == '.') == 3)
                    return assemblyVersion.Substring(0, assemblyVersion.LastIndexOf(".", StringComparison.Ordinal));
                return assemblyVersion;
            }
        }
    }
}
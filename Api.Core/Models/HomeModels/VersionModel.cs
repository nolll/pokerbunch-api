using System.Runtime.Serialization;

namespace Api.Models.HomeModels
{
    [DataContract(Namespace = "", Name = "application")]
    public class VersionModel
    {
        [DataMember(Name = "version")]
        public string Version { get; }
        
        public VersionModel(string version)
        {
            Version = version;
        }
    }
}
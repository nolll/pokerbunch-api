using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "player")]
    public class ApiPlayer
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public ApiPlayer(string name)
        {
            Name = name;
        }

        public ApiPlayer()
        {
        }
    }
}
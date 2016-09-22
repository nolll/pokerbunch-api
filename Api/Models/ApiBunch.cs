using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class ApiBunch
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "slug")]
        public string Slug { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public ApiBunch(int id, string slug, string name)
        {
            Id = id;
            Slug = slug;
            Name = name;
        }

        public ApiBunch()
        {
        }
    }
}
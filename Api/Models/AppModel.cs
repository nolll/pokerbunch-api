using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "app")]
    public class AppModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }

        public AppModel(AppResult app)
            : this(app.AppId, app.AppName, app.AppKey)
        {
        }

        private AppModel(int id, string name, string key)
        {
            Id = id;
            Name = name;
            Key = key;
        }

        public AppModel()
        {
        }
    }
}
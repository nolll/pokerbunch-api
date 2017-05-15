using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.AppModels
{
    [DataContract(Namespace = "", Name = "app")]
    public class AppModel
    {
        [DataMember(Name = "id")]
        public int Id { get; }
        [DataMember(Name = "name")]
        public string Name { get; }
        [DataMember(Name = "key")]
        public string Key { get; }

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
    }
}
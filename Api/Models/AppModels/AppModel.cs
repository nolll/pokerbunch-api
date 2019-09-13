using System.Runtime.Serialization;
using Api.Urls.ApiUrls;
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
        [DataMember(Name = "url")]
        public string Url { get; }

        public AppModel(AppResult app, UrlProvider urls)
            : this(app.AppId, app.AppName, app.AppKey, urls.Api.App(app.AppId.ToString()).Absolute())
        {
        }

        private AppModel(int id, string name, string key, string url)
        {
            Id = id;
            Name = name;
            Key = key;
            Url = url;
        }
    }
}
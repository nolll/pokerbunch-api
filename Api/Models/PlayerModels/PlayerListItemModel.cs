using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.PlayerModels
{
    [DataContract(Namespace = "", Name = "player")]
    public class PlayerListItemModel
    {
        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "userId")]
        public string UserId { get; }

        [DataMember(Name = "userName")]
        public string UserName { get; }

        [DataMember(Name = "color")]
        public string Color { get; }

        public PlayerListItemModel(GetPlayerList.ResultItem r)
        {
            Id = r.Id;
            Name = r.Name;
            Color = r.Color;
            UserId = r.UserId;
            UserName = r.UserName;
        }
    }
}
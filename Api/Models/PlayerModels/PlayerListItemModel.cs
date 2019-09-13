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

        //[DataMember(Name = "canDelete")]
        //public bool CanDelete { get; }

        //[DataMember(Name = "isUser")]
        //public bool IsUser { get; }

        //[DataMember(Name = "userName")]
        //public string UserName { get; }

        //[DataMember(Name = "avatarUrl")]
        //public string AvatarUrl { get; }

        [DataMember(Name = "color")]
        public string Color { get; }

        public PlayerListItemModel(GetPlayerList.ResultItem r)
        {
            Id = r.Id;
            Name = r.Name;
            Color = r.Color;
        }
    }
}
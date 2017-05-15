using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.PlayerModels
{
    [DataContract(Namespace = "", Name = "player")]
    public class PlayerModel
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

        //[DataMember(Name = "slug")]
        //public string Slug { get; }

        [DataMember(Name = "color")]
        public string Color { get; }

        public PlayerModel(GetPlayer.Result r)
            //: this(r.PlayerId, r.DisplayName, r.CanDelete, r.IsUser, r.UserName, r.AvatarUrl, r.Slug, r.Color)
            : this(r.PlayerId, r.DisplayName, r.Color)
        {
        }

        public PlayerModel(GetPlayerList.ResultItem r)
            : this(r.Id, r.Name, r.Color)
        {
        }

        // private PlayerModel(int id, string name, string color, bool canDelete, bool isUser, string userName, string avatarUrl, string slug)
        private PlayerModel(int id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
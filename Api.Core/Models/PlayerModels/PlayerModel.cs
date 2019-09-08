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

        [DataMember(Name = "userId")]
        public string UserId { get; }

        [DataMember(Name = "userName")]
        public string UserName { get; }

        [DataMember(Name = "avatarUrl")]
        public string AvatarUrl { get; }

        [DataMember(Name = "bunchId")]
        public string Slug { get; }

        [DataMember(Name = "color")]
        public string Color { get; }

        public PlayerModel(GetPlayer.Result r)
        {
            Id = r.PlayerId;
            Name = r.DisplayName;
            Slug = r.Slug;
            UserId = r.UserId.ToString();
            UserName = r.UserName;
            AvatarUrl = r.AvatarUrl;
            Color = r.Color;
        }
    }
}
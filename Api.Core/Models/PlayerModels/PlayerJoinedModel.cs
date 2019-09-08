using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.PlayerModels
{
    [DataContract(Namespace = "", Name = "player")]
    public class PlayerJoinedModel : MessageModel
    {
        private readonly int _id;
        public override string Message => $"Player joined {_id}";

        public PlayerJoinedModel(int id)
        {
            _id = id;
        }
    }
}
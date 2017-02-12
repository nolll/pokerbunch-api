using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameDeletedModel
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public CashgameDeletedModel(int id)
        {
            Message = $"Cashgame deleted{id}";
        }

        public CashgameDeletedModel()
        {
        }
    }
}
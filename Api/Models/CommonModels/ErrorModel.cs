using System.Runtime.Serialization;

namespace Api.Models.CommonModels
{
    [DataContract(Namespace = "", Name = "error")]
    public class ErrorModel : MessageModel
    {
        public override string Message { get; }

        public ErrorModel(string message)
        {
            Message = message;
        }
    }
}
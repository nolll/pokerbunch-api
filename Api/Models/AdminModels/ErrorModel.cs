using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.AdminModels
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
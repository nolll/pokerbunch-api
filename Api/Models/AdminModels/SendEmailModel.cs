using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.AdminModels
{
    [DataContract(Namespace = "", Name = "emailsent")]
    public class SendEmailModel
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public SendEmailModel(TestEmail.Result result)
        {
            var email = result.Email;
            Message = $"An email was sent to {email}";
        }

        public SendEmailModel()
        {
        }
    }
}
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "emailsent")]
    public class EmailSentModel
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public EmailSentModel(TestEmail.Result result)
        {
            var email = result.Email;
            Message = $"An email was sent to {email}";
        }

        public EmailSentModel()
        {
        }
    }
}
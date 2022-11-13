using System.Runtime.Serialization;
using Api.Models.CommonModels;
using Core.UseCases;

namespace Api.Models.AdminModels;

public class SendEmailModel : MessageModel
{
    private readonly string _email;

    public override string Message => $"An email was sent to {_email}";

    public SendEmailModel(TestEmail.Result result)
    {
        _email = result.Email;
    }
}
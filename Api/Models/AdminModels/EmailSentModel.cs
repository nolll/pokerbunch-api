using Api.Models.CommonModels;
using Core.UseCases;

namespace Api.Models.AdminModels;

public class EmailSentModel : MessageModel
{
    private readonly string _email;

    public override string Message => $"An email was sent to {_email}";

    public EmailSentModel(TestEmail.Result result)
    {
        _email = result.Email;
    }
}
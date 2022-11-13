namespace Api.Models.CommonModels;

public class ErrorModel : MessageModel
{
    public override string Message { get; }

    public ErrorModel(string message)
    {
        Message = message;
    }
}
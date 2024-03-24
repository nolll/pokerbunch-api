namespace Api.Models.CommonModels;

public class ErrorModel(string message) : MessageModel
{
    public override string Message { get; } = message;
}
namespace Core.Messages;

public interface IMessage
{
    string Subject { get; }
    string Body { get; }
}
namespace Core
{
    public interface IMessage
    {
        string Subject { get; }
        string Body { get; }
    }
}
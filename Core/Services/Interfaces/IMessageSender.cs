namespace Core.Services
{
    public interface IMessageSender
    {
        void Send(string to, IMessage message);
    }
}
namespace Core.Services;

public interface IEmailSender
{
    void Send(string to, IMessage message);
}
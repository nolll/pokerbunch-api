using Core.Messages;

namespace Core.Services;

public interface IEmailSender
{
    Task SendAsync(string to, IMessage message);
}
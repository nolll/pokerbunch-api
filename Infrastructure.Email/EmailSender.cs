using Core;
using Core.Services;

namespace Infrastructure.Email;

public abstract class EmailSender : IEmailSender
{
    protected const string FromEmail = "noreply@pokerbunch.com";
    protected const string FromName = "PokerBunch.com";

    public abstract void Send(string to, IMessage message);
}
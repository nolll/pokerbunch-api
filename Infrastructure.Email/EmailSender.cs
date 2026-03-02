using System.Threading.Tasks;
using Core.Messages;
using Core.Services;

namespace Infrastructure.Email;

public abstract class EmailSender : IEmailSender
{
    protected const string FromEmail = "noreply@pokerbunch.com";
    protected const string FromName = "PokerBunch.com";
    
    public abstract Task SendAsync(string to, IMessage message);
}
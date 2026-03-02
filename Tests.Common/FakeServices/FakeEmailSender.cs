using Core.Messages;
using Core.Services;

namespace Tests.Common.FakeServices;

public class FakeEmailSender : IEmailSender
{
    public string? LastSentTo { get; private set; }
    public IMessage? LastMessage { get; private set; }
    
    public Task SendAsync(string to, IMessage message)
    {
        LastSentTo = to;
        LastMessage = message;
        return Task.CompletedTask;
    }
}

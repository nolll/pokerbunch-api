using Core;
using Core.Services;

namespace Tests.Common.FakeServices;

public class FakeEmailSender : IEmailSender
{
    public string? To { get; private set; }
    public IMessage? Message { get; private set; }
    public IMessage? LastMessage { get; private set; }

    public void Send(string to, IMessage message)
    {
        To = to;
        Message = message;
        LastMessage = message;
    }

    public void Reset()
    {
        To = null;
        Message = null;
        LastMessage = null;
    }
}

using Core;
using Core.Services;

namespace Tests.Common.FakeServices;

public class FakeEmailSender : IEmailSender
{
    public string? To { get; private set; }
    public IMessage? LastMessage { get; private set; }

    public void Send(string to, IMessage message)
    {
        To = to;
        LastMessage = message;
    }

    public void Reset()
    {
        To = null;
        LastMessage = null;
    }
}

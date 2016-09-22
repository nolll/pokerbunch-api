using Core;
using Core.Services;

namespace Tests.Common.FakeServices
{
    public class FakeMessageSender : IMessageSender
    {
        public string To { get; private set; }
        public IMessage Message { get; private set; }

        public void Send(string to, IMessage message)
        {
            To = to;
            Message = message;
        }

        public void Reset()
        {
            To = null;
            Message = null;
        }
    }
}
using Tests.Common.FakeServices;

namespace Tests.Common
{
    public class ServiceContainer
    {
        public FakeMessageSender MessageSender { get; }
        public FakeRandomService RandomService { get; }

        public ServiceContainer(RepositoryContainer repos)
        {
            MessageSender = new FakeMessageSender();
            RandomService = new FakeRandomService();
        }

        public void Clear()
        {
            MessageSender.Reset();
        }
    }
}
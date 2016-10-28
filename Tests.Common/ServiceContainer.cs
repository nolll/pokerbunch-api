using Core.Services;
using Tests.Common.FakeServices;

namespace Tests.Common
{
    public class ServiceContainer
    {
        public FakeMessageSender MessageSender { get; }
        public FakeRandomService RandomService { get; }
        public CashgameService CashgameService { get; }
        public PlayerService PlayerService { get; }

        public ServiceContainer(RepositoryContainer repos)
        {
            MessageSender = new FakeMessageSender();
            RandomService = new FakeRandomService();
            CashgameService = new CashgameService(repos.Cashgame);
            PlayerService = new PlayerService(repos.Player);
        }

        public void Clear()
        {
            MessageSender.Reset();
        }
    }
}
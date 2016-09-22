using Tests.Common.FakeRepositories;

namespace Tests.Common
{
    public class RepositoryContainer
    {
        public FakeBunchRepository Bunch { get; private set; }
        public FakeUserRepository User { get; private set; }
        public FakePlayerRepository Player { get; private set; }
        public FakeCashgameRepository Cashgame { get; private set; }
        public FakeEventRepository Event { get; private set; }
        public FakeLocationRepository Location { get; private set; }
        public FakeAppRepository App { get; private set; }

        public RepositoryContainer()
        {
            Bunch = new FakeBunchRepository();
            User = new FakeUserRepository();
            Player = new FakePlayerRepository();
            Cashgame = new FakeCashgameRepository();
            Event = new FakeEventRepository();
            Location = new FakeLocationRepository();
            App = new FakeAppRepository();
        }
    }
}
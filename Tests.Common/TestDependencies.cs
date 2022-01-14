using Tests.Common.FakeRepositories;
using Tests.Common.FakeServices;

namespace Tests.Common;

public class TestDependencies
{
    public FakeBunchRepository Bunch { get; }
    public FakeUserRepository User { get; }
    public FakePlayerRepository Player { get; }
    public FakeCashgameRepository Cashgame { get; }
    public FakeEventRepository Event { get; }
    public FakeLocationRepository Location { get; }

    public FakeEmailSender EmailSender { get; }
    public FakeRandomizer Randomizer { get; }

    public TestDependencies()
    {
        Bunch = new FakeBunchRepository();
        User = new FakeUserRepository();
        Player = new FakePlayerRepository();
        Cashgame = new FakeCashgameRepository();
        Event = new FakeEventRepository();
        Location = new FakeLocationRepository();

        EmailSender = new FakeEmailSender();
        Randomizer = new FakeRandomizer();
    }

    public void Clear()
    {
        EmailSender.Reset();
    }
}
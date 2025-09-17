using Tests.Common.FakeRepositories;
using Tests.Common.FakeServices;

namespace Tests.Common;

public class TestDependencies
{
    public FakeBunchRepository Bunch { get; } = new();
    public FakeUserRepository User { get; } = new();
    public FakePlayerRepository Player { get; } = new();
    public FakeCashgameRepository Cashgame { get; } = new();
    public FakeEventRepository Event { get; } = new();
    public FakeLocationRepository Location { get; } = new();
}
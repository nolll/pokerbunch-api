using System;
using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CashgameDetailsTests : TestBase
{
    [Test]
    public async Task CashgameDetails_CashgameRunning_AllSimplePropertiesAreSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, TestData.PlayerIdA, "", Role.Player);
        var request = new CashgameDetails.Request(new AuthInTest(canSeeCashgame: true, userBunch: userBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        result.Data!.PlayerId.Should().Be(TestData.PlayerIdA);
        result.Data!.LocationName.Should().Be(TestData.LocationNameC);
        result.Data!.DefaultBuyin.Should().Be(100);
        result.Data!.Role.Should().Be(Role.Player);
    }
        
    [Test]
    public async Task CashgameDetails_CashgameRunning_SlugIsSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName);
        var request = new CashgameDetails.Request(new AuthInTest(canSeeCashgame: true, userBunch: userBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        result.Data!.Slug.Should().Be("bunch-a");
    }

    [Test]
    public async Task CashgameDetails_CashgameRunning_PlayerItemsAreSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName);
        var request = new CashgameDetails.Request(new AuthInTest(canSeeCashgame: true, userBunch: userBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        result.Data!.PlayerItems.Count.Should().Be(2);
        result.Data!.PlayerItems[0].Checkpoints.Count.Should().Be(1);
        result.Data!.PlayerItems[0].HasCashedOut.Should().BeFalse();
        result.Data!.PlayerItems[0].Name.Should().Be(TestData.PlayerA.DisplayName);
        result.Data!.PlayerItems[0].PlayerId.Should().Be(TestData.PlayerA.Id);
        result.Data!.PlayerItems[0].CashgameId.Should().Be("3");
        result.Data!.PlayerItems[0].PlayerId.Should().Be("1");
        result.Data!.PlayerItems[1].Checkpoints.Count.Should().Be(1);
        result.Data!.PlayerItems[1].HasCashedOut.Should().BeFalse();
        result.Data!.PlayerItems[1].Name.Should().Be(TestData.PlayerB.DisplayName);
        result.Data!.PlayerItems[1].PlayerId.Should().Be(TestData.PlayerB.Id);
        result.Data!.PlayerItems[1].CashgameId.Should().Be("3");
        result.Data!.PlayerItems[1].PlayerId.Should().Be("2");
    }

    private CashgameDetails Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.Player,
        Deps.Location,
        Deps.Event);
}
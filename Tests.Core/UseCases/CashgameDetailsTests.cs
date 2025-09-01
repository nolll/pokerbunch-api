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

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, TestData.PlayerIdA, "", Role.Player);
        var request = new CashgameDetails.Request(new PrincipalInTest(canSeeCashgame: true, currentBunch: currentBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.PlayerId, Is.EqualTo(TestData.PlayerIdA));
        Assert.That(result.Data?.LocationName, Is.EqualTo(TestData.LocationNameC));
        Assert.That(result.Data?.DefaultBuyin, Is.EqualTo(100));
        Assert.That(result.Data?.Role, Is.EqualTo(Role.Player));
    }
        
    [Test]
    public async Task CashgameDetails_CashgameRunning_SlugIsSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, "", "", Role.None);
        var request = new CashgameDetails.Request(new PrincipalInTest(canSeeCashgame: true, currentBunch: currentBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Slug, Is.EqualTo("bunch-a"));
    }

    [Test]
    public async Task CashgameDetails_CashgameRunning_PlayerItemsAreSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, "", "", Role.None);
        var request = new CashgameDetails.Request(new PrincipalInTest(canSeeCashgame: true, currentBunch: currentBunch), TestData.CashgameIdC, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.PlayerItems.Count, Is.EqualTo(2));
        Assert.That(result.Data?.PlayerItems[0].Checkpoints.Count, Is.EqualTo(1));
        Assert.That(result.Data?.PlayerItems[0].HasCashedOut, Is.False);
        Assert.That(result.Data?.PlayerItems[0].Name, Is.EqualTo(TestData.PlayerA.DisplayName));
        Assert.That(result.Data?.PlayerItems[0].PlayerId, Is.EqualTo(TestData.PlayerA.Id));
        Assert.That(result.Data?.PlayerItems[0].CashgameId, Is.EqualTo("3"));
        Assert.That(result.Data?.PlayerItems[0].PlayerId, Is.EqualTo("1"));
        Assert.That(result.Data?.PlayerItems[1].Checkpoints.Count, Is.EqualTo(1));
        Assert.That(result.Data?.PlayerItems[1].HasCashedOut, Is.False);
        Assert.That(result.Data?.PlayerItems[1].Name, Is.EqualTo(TestData.PlayerB.DisplayName));
        Assert.That(result.Data?.PlayerItems[1].PlayerId, Is.EqualTo(TestData.PlayerB.Id));
        Assert.That(result.Data?.PlayerItems[1].CashgameId, Is.EqualTo("3"));
        Assert.That(result.Data?.PlayerItems[1].PlayerId, Is.EqualTo("2"));
    }

    private CashgameDetails Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.Player,
        Deps.Location,
        Deps.Event);
}
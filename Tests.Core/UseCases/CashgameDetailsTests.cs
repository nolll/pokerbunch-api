using System;
using Core.Entities;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class CashgameDetailsTests : TestBase
{
    [Test]
    public void RunningCashgame_CashgameNotRunning_ThrowsException()
    {
        //var request = new CashgameDetails2.Request(TestData.UserNameA, TestData.CashgameIdC);

        //Assert.Throws<CashgameNotRunningException>(() => Sut.Execute(request));
    }

    [Test]
    public void CashgameDetails_CashgameRunning_AllSimplePropertiesAreSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC, DateTime.UtcNow);
        var result = Sut.Execute(request);

        Assert.AreEqual(TestData.PlayerIdA, result.Data.PlayerId);
        Assert.AreEqual(TestData.LocationNameC, result.Data.LocationName);
        Assert.AreEqual(100, result.Data.DefaultBuyin);
        Assert.AreEqual(Role.Player, result.Data.Role);
    }

    [Test]
    public void CashgameDetails_CashgameRunning_SlugIsSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC, DateTime.UtcNow);
        var result = Sut.Execute(request);

        Assert.AreEqual("bunch-a", result.Data.Slug);
    }

    [Test]
    public void CashgameDetails_CashgameRunning_PlayerItemsAreSet()
    {
        Deps.Cashgame.SetupRunningGame();

        var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC, DateTime.UtcNow);
        var result = Sut.Execute(request);

        Assert.AreEqual(2, result.Data.PlayerItems.Count);
        Assert.AreEqual(1, result.Data.PlayerItems[0].Checkpoints.Count);
        Assert.IsFalse(result.Data.PlayerItems[0].HasCashedOut);
        Assert.AreEqual(TestData.PlayerA.DisplayName, result.Data.PlayerItems[0].Name);
        Assert.AreEqual(TestData.PlayerA.Id, result.Data.PlayerItems[0].PlayerId);
        Assert.AreEqual(3, result.Data.PlayerItems[0].CashgameId);
        Assert.AreEqual(1, result.Data.PlayerItems[0].PlayerId);
        Assert.AreEqual(1, result.Data.PlayerItems[1].Checkpoints.Count);
        Assert.IsFalse(result.Data.PlayerItems[1].HasCashedOut);
        Assert.AreEqual(TestData.PlayerB.DisplayName, result.Data.PlayerItems[1].Name);
        Assert.AreEqual(TestData.PlayerB.Id, result.Data.PlayerItems[1].PlayerId);
        Assert.AreEqual(3, result.Data.PlayerItems[1].CashgameId);
        Assert.AreEqual(2, result.Data.PlayerItems[1].PlayerId);
    }

    private CashgameDetails Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.Player,
        Deps.User,
        Deps.Location,
        Deps.Event);
}
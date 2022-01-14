using System;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class ActionsTests : TestBase
{
    private const string BuyinDescription = "Buyin";
    private const string CashoutDescription = "Cashout";

    [Test]
    public void Actions_ActionsResultIsReturned()
    {
        var request = new Actions.Request(TestData.UserA.UserName, TestData.CashgameIdA, TestData.PlayerIdA);
        var result = Sut.Execute(request);

        var actionTime = DateTime.SpecifyKind(DateTime.Parse("2001-01-01 12:00:00"), DateTimeKind.Utc);
        Assert.AreEqual(actionTime, result.Date);
        Assert.AreEqual(TestData.PlayerNameA, result.PlayerName);
        Assert.AreEqual(2, result.CheckpointItems.Count);
    }

    [Test]
    public void Actions_ItemPropertiesAreSet()
    {
        var request = new Actions.Request(TestData.UserA.UserName, TestData.CashgameIdA, TestData.PlayerIdA);
        var result = Sut.Execute(request);

        var actionTime = DateTime.SpecifyKind(DateTime.Parse("2001-01-01 12:00:00"), DateTimeKind.Utc);
        Assert.AreEqual(BuyinDescription, result.CheckpointItems[0].Type);
        Assert.AreEqual(200, result.CheckpointItems[0].DisplayAmount.Amount);
        Assert.AreEqual(actionTime, result.CheckpointItems[0].Time);
        Assert.IsFalse(result.CheckpointItems[0].CanEdit);
        Assert.AreEqual(1, result.CheckpointItems[0].CheckpointId);

        Assert.AreEqual(CashoutDescription, result.CheckpointItems[1].Type);
        Assert.AreEqual(50, result.CheckpointItems[1].DisplayAmount.Amount);
    }

    [Test]
    public void Actions_WithManager_CanEditIsTrueOnItem()
    {
        var request = new Actions.Request(TestData.UserC.UserName, TestData.CashgameIdA, TestData.PlayerA.Id);
        var result = Sut.Execute(request);

        Assert.IsTrue(result.CheckpointItems[0].CanEdit);
    }

    private Actions Sut => new Actions(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.Player,
        Deps.User);
}
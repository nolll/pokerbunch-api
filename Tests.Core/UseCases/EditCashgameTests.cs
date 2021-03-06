using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditCashgameTests : TestBase
{
    [Test]
    public void EditCashgame_EmptyLocation_ThrowsException()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, 0, 0);

        Assert.Throws<ValidationException>(() => Sut.Execute(request));
    }

    [Test]
    public void EditCashgame_ValidLocation_NoException()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 0);

        Sut.Execute(request);
    }

    [Test]
    public void EditCashgame_ValidLocation_SavesCashgame()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 0);

        Sut.Execute(request);

        Assert.AreEqual(TestData.BunchA.Id, Deps.Cashgame.Updated.Id);
        Assert.AreEqual(TestData.ChangedLocationId, Deps.Cashgame.Updated.LocationId);
    }

    [Test]
    public void EditCashgame_WithEventId_GameIsAddedToEvent()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 1);
        Sut.Execute(request);

        Assert.AreEqual(1, Deps.Event.AddedCashgameId);
    }

    private EditCashgame Sut => new EditCashgame(
        Deps.Cashgame,
        Deps.User,
        Deps.Player,
        Deps.Location,
        Deps.Event);
}
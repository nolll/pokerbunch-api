﻿using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditCashgameTests : TestBase
{
    [Test]
    public async Task EditCashgame_EmptyLocation_ReturnsError()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, 0, 0);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCashgame_ValidLocation_NoError()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 0);

        var result = await Sut.Execute(request);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task EditCashgame_ValidLocation_SavesCashgame()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 0);

        await Sut.Execute(request);

        Assert.AreEqual(TestData.BunchA.Id, Deps.Cashgame.Updated.Id);
        Assert.AreEqual(TestData.ChangedLocationId, Deps.Cashgame.Updated.LocationId);
    }

    [Test]
    public async Task EditCashgame_WithEventId_GameIsAddedToEvent()
    {
        var request = new EditCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA, TestData.ChangedLocationId, 1);
        await Sut.Execute(request);

        Assert.AreEqual(1, Deps.Event.AddedCashgameId);
    }

    private EditCashgame Sut => new(
        Deps.Cashgame,
        Deps.User,
        Deps.Player,
        Deps.Location,
        Deps.Event);
}
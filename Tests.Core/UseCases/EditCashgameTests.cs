using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EditCashgameTests : TestBase
{
    [Test]
    public async Task EditCashgame_EmptyLocation_ReturnsError()
    {
        var request = new EditCashgame.Request(new PrincipalInTest(canEditCashgame: true), TestData.CashgameIdA, null, null);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCashgame_ValidLocation_NoError()
    {
        var request = new EditCashgame.Request(new PrincipalInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, null);

        var result = await Sut.Execute(request);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task EditCashgame_ValidLocation_SavesCashgame()
    {
        var request = new EditCashgame.Request(new PrincipalInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, null);

        await Sut.Execute(request);

        Assert.That(Deps.Cashgame.Updated?.Id, Is.EqualTo(TestData.BunchA.Id));
        Assert.That(Deps.Cashgame.Updated?.LocationId, Is.EqualTo(TestData.ChangedLocationId));
    }

    [Test]
    public async Task EditCashgame_WithEventId_GameIsAddedToEvent()
    {
        var request = new EditCashgame.Request(new PrincipalInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, "1");
        await Sut.Execute(request);

        Assert.That(Deps.Event.AddedCashgameId, Is.EqualTo("1"));
    }

    private EditCashgame Sut => new(
        Deps.Cashgame,
        Deps.Location,
        Deps.Event);
}
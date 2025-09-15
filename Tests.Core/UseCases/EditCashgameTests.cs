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
        var request = new EditCashgame.Request(new AuthInTest(canEditCashgame: true), TestData.CashgameIdA, null, null);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditCashgame_ValidLocation_NoError()
    {
        var request = new EditCashgame.Request(new AuthInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, null);

        var result = await Sut.Execute(request);
        result.Success.Should().BeTrue();
    }

    [Test]
    public async Task EditCashgame_ValidLocation_SavesCashgame()
    {
        var request = new EditCashgame.Request(new AuthInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, null);

        await Sut.Execute(request);

        Deps.Cashgame.Updated!.Id.Should().Be(TestData.BunchA.Id);
        Deps.Cashgame.Updated!.LocationId.Should().Be(TestData.ChangedLocationId);
    }

    [Test]
    public async Task EditCashgame_WithEventId_GameIsAddedToEvent()
    {
        var request = new EditCashgame.Request(new AuthInTest(canEditCashgame: true), TestData.CashgameIdA, TestData.ChangedLocationId, "1");
        await Sut.Execute(request);

        Deps.Event.AddedCashgameId.Should().Be("1");
    }

    private EditCashgame Sut => new(
        Deps.Cashgame,
        Deps.Location,
        Deps.Event);
}
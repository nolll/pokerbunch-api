using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddCashgameTests : TestBase
{
    [Test]
    public async Task AddCashgame_SlugIsSet()
    {
        var request = CreateRequest(TestData.LocationIdA);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Slug, Is.EqualTo(TestData.SlugA));
    }

    [Test]
    public async Task AddCashgame_WithLocation_GameIsAdded()
    {
        var request = CreateRequest(TestData.LocationIdA);
        await Sut.Execute(request);

        Assert.That(Deps.Cashgame.Added, Is.Not.Null);
    }

    [Test]
    public async Task AddCashgame_WithoutLocation_ReturnsError()
    {
        var request = CreateRequest();
        var result = await Sut.Execute(request);
        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    private static AddCashgame.Request CreateRequest(string? locationId = null)
    {
        var currentBunch = new CurrentBunch(TestData.BunchIdA, TestData.SlugA, "", "", "", Role.None);
        return new AddCashgame.Request(new AccessControlInTest(canAddCashgame: true, currentBunch: currentBunch), TestData.SlugA, locationId);
    }

    private AddCashgame Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.Location);
}
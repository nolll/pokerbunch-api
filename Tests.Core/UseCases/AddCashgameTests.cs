using Core.Errors;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddCashgameTests : TestBase
{
    [Test]
    public void AddCashgame_SlugIsSet()
    {
        var request = CreateRequest(TestData.LocationIdA);
        var result = Sut.Execute(request);

        Assert.AreEqual(TestData.SlugA, result.Data.Slug);
    }

    [Test]
    public void AddCashgame_WithLocation_GameIsAdded()
    {
        var request = CreateRequest(TestData.LocationIdA);
        Sut.Execute(request);

        Assert.IsNotNull(Deps.Cashgame.Added);
    }

    [Test]
    public void AddCashgame_WithoutLocation_ReturnsError()
    {
        var request = CreateRequest();
        var result = Sut.Execute(request);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    private static AddCashgame.Request CreateRequest(int locationId = 0)
    {
        return new AddCashgame.Request(TestData.UserNameA, TestData.SlugA, locationId);
    }

    private AddCashgame Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.User,
        Deps.Player,
        Deps.Location);
}
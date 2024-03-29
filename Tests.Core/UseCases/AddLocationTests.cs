using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddLocationTests : TestBase
{
    [Test]
    public async Task AddLocation_AllOk_LocationIsAdded()
    {
        const string addedEventName = "added location";

        var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        await Sut.Execute(request);

        Assert.That(Deps.Location.Added?.Name, Is.EqualTo(addedEventName));
    }

    [Test]
    public async Task AddEvent_InvalidName_ReturnsError()
    {
        const string addedEventName = "";

        var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    private AddLocation Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User,
        Deps.Location);
}
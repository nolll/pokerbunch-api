using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddEventTests : TestBase
{
    [Test]
    public async Task AddEvent_AllOk_EventIsAdded()
    {
        const string addedEventName = "added event";

        var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        await Sut.Execute(request);

        Assert.That(Deps.Event.Added?.Name, Is.EqualTo(addedEventName));
    }

    [Test]
    public async Task AddEvent_InvalidName_ReturnsValidationError()
    {
        const string addedEventName = "";

        var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    private AddEvent Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User,
        Deps.Event);
}
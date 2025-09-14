using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddEventTests : TestBase
{
    [Test]
    public async Task AddEvent_AllOk_EventIsAdded()
    {
        const string addedEventName = "added event";

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.None);
        var request = new AddEvent.Request(new AuthInTest(canAddEvent: true, currentBunch: currentBunch), TestData.BunchA.Slug, addedEventName);
        await Sut.Execute(request);

        Deps.Event.Added?.Name.Should().Be(addedEventName);
    }

    [Test]
    public async Task AddEvent_InvalidName_ReturnsValidationError()
    {
        const string addedEventName = "";
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.None);
        var request = new AddEvent.Request(new AuthInTest(canAddEvent: true, currentBunch: currentBunch), TestData.BunchA.Slug, addedEventName);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    private AddEvent Sut => new(Deps.Event);
}
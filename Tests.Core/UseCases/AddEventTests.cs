using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddEventTests : TestBase
{
    [Test]
    public void AddEvent_AllOk_EventIsAdded()
    {
        const string addedEventName = "added event";

        var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        Sut.Execute(request);

        Assert.AreEqual(addedEventName, Deps.Event.Added.Name);
    }

    [Test]
    public void AddEvent_InvalidName_ReturnsValidationError()
    {
        const string addedEventName = "";

        var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    private AddEvent Sut => new AddEvent(
        Deps.Bunch,
        Deps.Player,
        Deps.User,
        Deps.Event);
}
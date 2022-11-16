using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddLocationTests : TestBase
{
    [Test]
    public void AddLocation_AllOk_LocationIsAdded()
    {
        const string addedEventName = "added location";

        var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        Sut.Execute(request);

        Assert.AreEqual(addedEventName, Deps.Location.Added.Name);
    }

    [Test]
    public void AddEvent_InvalidName_ThrowsValidationException()
    {
        const string addedEventName = "";

        var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    private AddLocation Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User,
        Deps.Location);
}
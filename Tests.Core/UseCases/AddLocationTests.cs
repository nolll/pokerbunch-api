using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddLocationTests : TestBase
{
    [Test]
    public async Task AddLocation_AllOk_LocationIsAdded()
    {
        const string addedEventName = "added location";

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.Manager);
        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true, currentBunch: currentBunch), TestData.BunchA.Slug, addedEventName);
        await Sut.Execute(request);

        Deps.Location.Added?.Name.Should().Be(addedEventName);
    }

    [Test]
    public async Task AddEvent_InvalidName_ReturnsError()
    {
        const string addedEventName = "";

        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true), TestData.BunchA.Slug, addedEventName);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    private AddLocation Sut => new(Deps.Location);
}
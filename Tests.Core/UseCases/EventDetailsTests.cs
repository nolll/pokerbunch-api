using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

class EventDetailsTests : TestBase
{
    [Test]
    public async Task EventDetails_NameIsSet()
    {
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.None);
        var input = new EventDetails.Request(new AccessControlInTest(canSeeEventDetails: true, currentBunch: currentBunch), "1");
        var result = await Sut.Execute(input);

        Assert.That(result.Data?.Name, Is.EqualTo(TestData.EventNameA));
    }

    private EventDetails Sut => new(
        Deps.Event,
        Deps.Location);
}
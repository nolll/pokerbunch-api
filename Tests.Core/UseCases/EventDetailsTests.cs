using Core.Entities;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EventDetailsTests : TestBase
{
    [Test]
    public async Task EventDetails_NameIsSet()
    {
        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug);
        var input = new EventDetails.Request(new AuthInTest(canSeeEventDetails: true, userBunch: userBunch), "1");
        var result = await Sut.Execute(input);

        result.Data!.Name.Should().Be(TestData.EventNameA);
    }

    private EventDetails Sut => new(
        Deps.Event,
        Deps.Location);
}
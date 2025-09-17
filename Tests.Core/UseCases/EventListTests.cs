using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EventListTests : TestBase
{
    [Test]
    public async Task EventList_ReturnsAllEvents()
    {
        var result = await Sut.Execute(CreateInput());

        result.Data!.Events.Count.Should().Be(2);
    }

    [Test]
    public async Task EventList_EachItem_NameIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        result.Data!.Events[0].Name.Should().Be(TestData.EventNameB);
        result.Data!.Events[1].Name.Should().Be(TestData.EventNameA);
    }

    [Test]
    public async Task EventList_EachItem_StartTimeIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        result.Data!.Events[0].StartDate.Should().Be(new Date(2002, 2, 2));
        result.Data!.Events[1].StartDate.Should().Be(new Date(2001, 1, 1));
    }

    [Test]
    public async Task EventList_EachItem_UrlIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        result.Data!.Events[0].EventId.Should().Be("2");
        result.Data!.Events[1].EventId.Should().Be("1");
    }

    private EventList Sut => new(
        Deps.Event,
        Deps.Location);

    private EventList.Request CreateInput()
    {
        var userBunch = Create.UserBunch(TestData.BunchIdA, TestData.SlugA);
        return new EventList.Request(new AuthInTest(canListEvents: true, userBunch: userBunch), TestData.SlugA);
    }
}
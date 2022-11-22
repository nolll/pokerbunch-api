using Core.Entities;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class EventListTests : TestBase
{
    [Test]
    public async Task EventList_ReturnsAllEvents()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data.Events.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task EventList_EachItem_NameIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data.Events[0].Name, Is.EqualTo(TestData.EventNameB));
        Assert.That(result.Data.Events[1].Name, Is.EqualTo(TestData.EventNameA));
    }

    [Test]
    public async Task EventList_EachItem_StartTimeIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data.Events[0].StartDate, Is.EqualTo(new Date(2002, 2, 2)));
        Assert.That(result.Data.Events[1].StartDate, Is.EqualTo(new Date(2001, 1, 1)));
    }

    [Test]
    public async Task EventList_EachItem_UrlIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data.Events[0].EventId, Is.EqualTo("2"));
        Assert.That(result.Data.Events[1].EventId, Is.EqualTo("1"));
    }

    private EventList Sut => new(
        Deps.Bunch,
        Deps.Event,
        Deps.User,
        Deps.Player,
        Deps.Location);

    private EventList.Request CreateInput()
    {
        return new EventList.Request(TestData.UserNameA, TestData.SlugA);
    }
}
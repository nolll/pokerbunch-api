using Core.Entities;
using Core.Services;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

class EventListTests : TestBase
{
    [Test]
    public async Task EventList_ReturnsAllEvents()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data?.Events.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task EventList_EachItem_NameIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data?.Events[0].Name, Is.EqualTo(TestData.EventNameB));
        Assert.That(result.Data?.Events[1].Name, Is.EqualTo(TestData.EventNameA));
    }

    [Test]
    public async Task EventList_EachItem_StartTimeIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data?.Events[0].StartDate, Is.EqualTo(new Date(2002, 2, 2)));
        Assert.That(result.Data?.Events[1].StartDate, Is.EqualTo(new Date(2001, 1, 1)));
    }

    [Test]
    public async Task EventList_EachItem_UrlIsSet()
    {
        var result = await Sut.Execute(CreateInput());

        Assert.That(result.Data?.Events[0].EventId, Is.EqualTo("2"));
        Assert.That(result.Data?.Events[1].EventId, Is.EqualTo("1"));
    }

    private EventList Sut => new(
        Deps.Event,
        Deps.Location);

    private EventList.Request CreateInput()
    {
        var currentBunch = new CurrentBunch(TestData.BunchIdA, TestData.SlugA, "", "", "", Role.None);
        return new EventList.Request(new AuthInTest(canListEvents: true, currentBunch: currentBunch), TestData.SlugA);
    }
}
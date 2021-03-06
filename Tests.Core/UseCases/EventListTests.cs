using Core.Entities;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class EventListTests : TestBase
{
    [Test]
    public void EventList_ReturnsAllEvents()
    {
        var result = Sut.Execute(CreateInput());

        Assert.AreEqual(2, result.Events.Count);
    }

    [Test]
    public void EventList_EachItem_NameIsSet()
    {
        var result = Sut.Execute(CreateInput());

        Assert.AreEqual(TestData.EventNameB, result.Events[0].Name);
        Assert.AreEqual(TestData.EventNameA, result.Events[1].Name);
    }

    [Test]
    public void EventList_EachItem_StartTimeIsSet()
    {
        var result = Sut.Execute(CreateInput());

        Assert.AreEqual(new Date(2002, 2, 2), result.Events[0].StartDate);
        Assert.AreEqual(new Date(2001, 1, 1), result.Events[1].StartDate);
    }

    [Test]
    public void EventList_EachItem_UrlIsSet()
    {
        var result = Sut.Execute(CreateInput());

        Assert.AreEqual(2, result.Events[0].EventId);
        Assert.AreEqual(1, result.Events[1].EventId);
    }

    private EventList Sut => new EventList(
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
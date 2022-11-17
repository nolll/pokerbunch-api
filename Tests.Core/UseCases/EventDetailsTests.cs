using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class EventDetailsTests : TestBase
{
    [Test]
    public void EventDetails_NameIsSet()
    {
        var input = new EventDetails.Request(TestData.UserNameA, 1);
        var result = Sut.Execute(input);

        Assert.AreEqual(TestData.EventNameA, result.Data.Name);
    }

    private EventDetails Sut => new(
        Deps.Event,
        Deps.User,
        Deps.Player,
        Deps.Bunch,
        Deps.Location);
}
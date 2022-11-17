using Core.UseCases;
using NUnit.Framework;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.Core.UseCases;

class EventDetailsTests : TestBase
{
    [Test]
    public async Task EventDetails_NameIsSet()
    {
        var input = new EventDetails.Request(TestData.UserNameA, 1);
        var result = await Sut.Execute(input);

        Assert.AreEqual(TestData.EventNameA, result.Data.Name);
    }

    private EventDetails Sut => new(
        Deps.Event,
        Deps.User,
        Deps.Player,
        Deps.Bunch,
        Deps.Location);
}
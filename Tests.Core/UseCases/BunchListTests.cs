using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class BunchListTests : TestBase
{
    [Test]
    public async Task BunchList_ReturnsListOfBunchItems()
    {
        var result = await Sut.Execute(new GetBunchList.Request(new AuthInTest(canListBunches: true)));

        Assert.That(result.Data?.Bunches.Count, Is.EqualTo(2));
        Assert.That(result.Data?.Bunches[0].Slug, Is.EqualTo("bunch-a"));
        Assert.That(result.Data?.Bunches[0].Name, Is.EqualTo(TestData.BunchA.DisplayName));
        Assert.That(result.Data?.Bunches[1].Slug, Is.EqualTo("bunch-b"));
        Assert.That(result.Data?.Bunches[1].Name, Is.EqualTo(TestData.BunchB.DisplayName));
    }

    private GetBunchList Sut => new(Deps.Bunch);
}
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

        result.Data!.Bunches.Count.Should().Be(2);
        result.Data!.Bunches[0].Slug.Should().Be("bunch-a");
        result.Data!.Bunches[0].Name.Should().Be(TestData.BunchA.DisplayName);
        result.Data!.Bunches[1].Slug.Should().Be("bunch-b");
        result.Data!.Bunches[1].Name.Should().Be(TestData.BunchB.DisplayName);
    }

    private GetBunchList Sut => new(Deps.Bunch);
}
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class BunchListTests : TestBase
{
    [Test]
    public void BunchList_ReturnsListOfBunchItems()
    {
        var result = Sut.Execute(new GetBunchList.Request(TestData.AdminUser.UserName));

        Assert.AreEqual(2, result.Data.Bunches.Count);
        Assert.AreEqual("bunch-a", result.Data.Bunches[0].Slug);
        Assert.AreEqual(TestData.BunchA.DisplayName, result.Data.Bunches[0].Name);
        Assert.AreEqual("bunch-b", result.Data.Bunches[1].Slug);
        Assert.AreEqual(TestData.BunchB.DisplayName, result.Data.Bunches[1].Name);
    }

    private GetBunchList Sut => new(
        Deps.Bunch,
        Deps.User);
}
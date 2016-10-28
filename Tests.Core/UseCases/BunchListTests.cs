using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class BunchListTests : TestBase
    {
        [Test]
        public void BunchList_ReturnsListOfBunchItems()
        {
            var result = Sut.Execute(new GetBunchList.AllBunchesRequest(TestData.AdminUser.UserName));

            Assert.AreEqual(2, result.Bunches.Count);
            Assert.AreEqual("bunch-a", result.Bunches[0].Slug);
            Assert.AreEqual(TestData.BunchA.DisplayName, result.Bunches[0].Name);
            Assert.AreEqual("bunch-b", result.Bunches[1].Slug);
            Assert.AreEqual(TestData.BunchB.DisplayName, result.Bunches[1].Name);
        }

        private GetBunchList Sut => new GetBunchList(Repos.Bunch, Repos.User);
    }
}

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
            var result = Sut.Execute(new BunchList.AllBunchesRequest(TestData.AdminUser.UserName));

            Assert.AreEqual(2, result.Bunches.Count);
            Assert.AreEqual("bunch-a", result.Bunches[0].Slug);
            Assert.AreEqual(TestData.BunchA.DisplayName, result.Bunches[0].DisplayName);
            Assert.AreEqual("bunch-b", result.Bunches[1].Slug);
            Assert.AreEqual(TestData.BunchB.DisplayName, result.Bunches[1].DisplayName);
        }

        private BunchList Sut
        {
            get { return new BunchList(Services.BunchService, Services.UserService); }
        }
    }
}

using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class BunchContextTests : TestBase
    {
        [Test]
        public void BunchContext_WithSlug_HasBunchIsTrue()
        {
            var result = Sut.Execute(new BunchContext.BunchRequest(TestData.UserA.UserName, TestData.SlugA));

            Assert.IsTrue(result.HasBunch);
        }

        [Test]
        public void BunchContext_OneBunchWithoutSlug_HasBunchIsTrue()
        {
            Deps.Bunch.SetupOneBunchList();

            var result = Sut.Execute(new BunchContext.BunchRequest(TestData.UserA.UserName));

            Assert.IsTrue(result.HasBunch);
        }

        [Test]
        public void BunchContext_WithoutSlugAndBunches_HasBunchIsFalse()
        {
            Deps.Bunch.ClearList();

            var result = Sut.Execute(new BunchContext.BunchRequest(TestData.UserA.UserName));

            Assert.IsFalse(result.HasBunch);
        }

        [Test]
        public void Execute_AppContextIsSet()
        {
            var cashgameContextRequest = new BunchContext.BunchRequest(TestData.UserA.UserName, TestData.SlugA);

            var result = Sut.Execute(cashgameContextRequest);

            Assert.IsInstanceOf<CoreContext.Result>(result.AppContext);
        }

        private BunchContext Sut => new BunchContext(
            Deps.User,
            Deps.Bunch);
    }
}
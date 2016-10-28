using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class JoinBunchFormTests : TestBase
    {
        [Test]
        public void JoinBunchForm_BunchNameIsSet()
        {
            var request = new JoinBunchForm.Request(TestData.SlugA);

            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.BunchA.DisplayName, result.BunchName);
        }

        private JoinBunchForm Sut => new JoinBunchForm(Repos.Bunch);
    }
}

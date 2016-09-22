using Core.UseCases;
using NUnit.Framework;
using Tests.Common;
using System.Linq;

namespace Tests.Core.UseCases
{
    class BaseContextTests : TestBase
    {
        [Test]
        public void BaseContext_VersionIsSet()
        {
            var result = Sut.Execute();
            var numberOfDots = result.Version.Count(o => o == '.');

            Assert.AreEqual(2, numberOfDots);
        }

        private BaseContext Sut => new BaseContext();
    }
}
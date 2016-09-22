using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class VerifyAppKeyTests : TestBase
    {
        [Test]
        public void VerifyAppKey_ValidKey_IsValid()
        {
            var request = new VerifyAppKey.Request(TestData.AppA.AppKey);
            var result = Sut.Execute(request);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void VerifyAppKey_InvalidKey_IsInvalid()
        {
            var request = new VerifyAppKey.Request("invalid key");
            var result = Sut.Execute(request);

            Assert.IsFalse(result.IsValid);
        }

        private VerifyAppKey Sut
        {
            get
            {
                return new VerifyAppKey(Services.AppService);
            }
        }
    }
}
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AppDetailsTests : TestBase
    {
        [Test]
        public void AppDetails_AllDataIsSet()
        {
            var request = new GetApp.Request(TestData.AppA.Id);
            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.AppA.Id, result.AppId);
            Assert.AreEqual(TestData.AppA.AppKey, result.AppKey);
            Assert.AreEqual(TestData.AppA.Name, result.AppName);
        }

        private GetApp Sut => new GetApp(Repos.App);
    }
}
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AddAppTests : TestBase
    {
        [Test]
        public void AddApp_FirstTest()
        {
            const string addedAppName = "added app";

            Sut.Execute(new AddApp.Request(TestData.UserA.UserName, addedAppName));

            Assert.AreEqual(addedAppName, Repos.App.Added.Name);
            Assert.AreEqual(TestData.UserA.Id, Repos.App.Added.UserId);
        }

        private AddApp Sut => new AddApp(Repos.App, Repos.User);
    }
}

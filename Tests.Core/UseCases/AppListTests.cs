using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AppListTests : TestBase
    {
        [Test]
        public void AppList_AllAppsWithNonAdminUser_AccessDenied()
        {
            var request = new AppList.AllAppsRequest(TestData.UserA.UserName);

            Assert.Throws<AccessDeniedException>(() => Sut.Execute(request));
        }

        [Test]
        public void AppList_AllAppsWithAdminUser_AccessDenied()
        {
            var request = new AppList.AllAppsRequest(TestData.AdminUser.UserName);

            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual(TestData.AppA.Name, result.Items[0].AppName);
            Assert.AreEqual(TestData.AppB.Name, result.Items[1].AppName);
        }

        private AppList Sut => new AppList(Repos.App, Repos.User);
    }
}
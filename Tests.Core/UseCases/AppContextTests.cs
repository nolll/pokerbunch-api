using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class AppContextTests : TestBase
    {
        [Test]
        public void AppContext_WithoutUserName_AllPropertiesAreSet()
        {
            var result = Sut.Execute(new CoreContext.Request(null));

            Assert.IsFalse(result.IsLoggedIn);
            Assert.IsEmpty(result.UserDisplayName);
        }

        [Test]
        public void AppContext_WithUserName_LoggedInPropertiesAreSet()
        {
            var result = Sut.Execute(new CoreContext.Request(TestData.UserA.UserName));

            Assert.IsTrue(result.IsLoggedIn);
            Assert.AreEqual(TestData.UserDisplayNameA, result.UserDisplayName);
            Assert.AreEqual("user-name-a", result.UserName);
        }

        [Test]
        public void AppContext_WithInvalidUserName_LoggedInPropertiesAreSet()
        {
            var request = new CoreContext.Request("1");
            Assert.Throws<NotLoggedInException>(() => Sut.Execute(request));
        }

        private CoreContext Sut => new CoreContext(Repos.User);
    }
}
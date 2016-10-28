using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class LoginTests : TestBase
    {
        [Test]
        public void Login_UserNotFound_ThrowsException()
        {
            var request = new Login.Request("username-that-does-not-exist", "");

            Assert.Throws<LoginException>(() => Sut.Execute(request));
        }

        [Test]
        public void Login_UserFoundButPasswordIsWrong_ThrowsException()
        {
            var request = new Login.Request(TestData.UserA.UserName, "wrong password");

            Assert.Throws<LoginException>(() => Sut.Execute(request));
        }

        [Test]
        public void Login_UserFoundAndPasswordIsCorrect_UserNameIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(TestData.UserA.UserName, result.UserName);
        }
      
        private static Login.Request CreateRequest()
        {
            return new Login.Request(TestData.UserA.UserName, TestData.UserPasswordA);
        }

        private Login Sut => new Login(Repos.User);
    }
}

using Core.UseCases;
using NUnit.Framework;

namespace Tests.Core.UseCases
{
    public class LoginFormTests
    {
        [Test]
        public void LoginForm_WithoutReturnUrl_ReturnUrlHomeUrl()
        {
            const string homeUrl = "/a";
            const string returnUrl = "";

            var request = new LoginForm.Request(homeUrl, returnUrl);

            var result = Sut.Execute(request);

            Assert.AreEqual(homeUrl, result.ReturnUrl);
        }

        [Test]
        public void LoginForm_WithReturnUrl_ReturnUrlIsSet()
        {
            const string homeUrl = "/a";
            const string returnUrl = "/b";
            var request = new LoginForm.Request(homeUrl, returnUrl);

            var result = Sut.Execute(request);

            Assert.AreEqual(returnUrl, result.ReturnUrl);
        }

        private LoginForm Sut
        {
            get { return new LoginForm(); }
        }
    }
}

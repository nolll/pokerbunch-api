using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class TestEmailTests : TestBase
    {
        private const string Email = "henriks@gmail.com";

        [Test]
        public void TestEmail_MessageIsSent()
        {
            Sut.Execute(new TestEmail.Request(TestData.AdminUser.UserName));

            Assert.AreEqual(Email, Deps.MessageSender.To);
            Assert.AreEqual("Test Email", Deps.MessageSender.Message.Subject);
            Assert.AreEqual("This is a test email from pokerbunch.com", Deps.MessageSender.Message.Body);
        }

        [Test]
        public void TestEmail_EmailIsSet()
        {
            var result = Sut.Execute(new TestEmail.Request(TestData.AdminUser.UserName));

            Assert.AreEqual(Email, result.Email);
        }

        private TestEmail Sut => new TestEmail(
            Deps.MessageSender,
            Deps.User);
    }
}

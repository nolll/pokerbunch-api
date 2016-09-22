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

            Assert.AreEqual(Email, Services.MessageSender.To);
            Assert.AreEqual("Test Email", Services.MessageSender.Message.Subject);
            Assert.AreEqual("This is a test email from pokerbunch.com", Services.MessageSender.Message.Body);
        }

        [Test]
        public void TestEmail_EmailIsSet()
        {
            var result = Sut.Execute(new TestEmail.Request(TestData.AdminUser.UserName));

            Assert.AreEqual(Email, result.Email);
        }

        private TestEmail Sut
        {
            get
            {
                return new TestEmail(
                    Services.MessageSender,
                    Services.UserService);
            }
        }
    }
}

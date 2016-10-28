using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class ForgotPasswordTests : TestBase
    {
        private const string ValidEmail = TestData.UserEmailA;
        private const string InvalidEmail = "";
        private const string NonExistingEmail = "a@b.com";

        [Test]
        public void ForgotPassword_WithInvalidEmail_ValidationExceptionIsThrown()
        {
            var request = CreateRequest(InvalidEmail);

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void ForgotPassword_UserNotFound_ThrowsException()
        {
            Assert.Throws<UserNotFoundException>(() => Sut.Execute(CreateRequest(NonExistingEmail)));
        }

        [Test]
        public void ForgotPassword_SendsPasswordEmail()
        {
            const string subject = "Poker Bunch Password Recovery";
            const string body = @"Here is your new password for Poker Bunch:
aaaaaaaa

Please sign in here: loginUrl";
            Sut.Execute(CreateRequest());

            Assert.AreEqual(ValidEmail, Services.MessageSender.To);
            Assert.AreEqual(subject, Services.MessageSender.Message.Subject);
            Assert.AreEqual(body, Services.MessageSender.Message.Body);
        }

        [Test]
        public void ForgotPassword_SavesUserWithNewPassword()
        {
            Sut.Execute(CreateRequest());

            var savedUser = Repos.User.Saved;
            Assert.AreEqual("0478095c8ece0bbc11f94663ac2c4f10b29666de", savedUser.EncryptedPassword);
            Assert.AreEqual("aaaaaaaaaa", savedUser.Salt);
        }

        private ForgotPassword.Request CreateRequest(string email = ValidEmail)
        {
            return new ForgotPassword.Request(email, "loginUrl");
        }

        private ForgotPassword Sut => new ForgotPassword(
            Repos.User,
            Services.MessageSender,
            Services.RandomService);
    }
}

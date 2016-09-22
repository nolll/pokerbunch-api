using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class ChangePasswordTests : TestBase
    {
        [Test]
        public void ChangePassword_EmptyPassword_ThrowsValidationException()
        {
            var request = new ChangePassword.Request(TestData.UserNameA, "", "");

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [Test]
        public void ChangePassword_DifferentPasswords_ThrowsValidationException()
        {
            var request = new ChangePassword.Request(TestData.UserNameA, "a", "b");

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [Test]
        public void ChangePassword_EqualPasswords_SavesUserWithNewPassword()
        {
            var request = new ChangePassword.Request(TestData.UserNameA, "a", "a");
            Sut.Execute(request);

            Assert.AreNotEqual(TestData.UserA.EncryptedPassword, Repos.User.Saved.EncryptedPassword);
        }
        
        private ChangePassword Sut
        {
            get
            {
                return new ChangePassword(
                    Services.UserService,
                    Services.RandomService);
            }
        }
    }
}
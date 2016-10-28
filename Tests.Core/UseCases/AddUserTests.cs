using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class AddUserTests : TestBase
    {
        private const string ValidUserName = "a";
        private const string ValidDisplayName = "b";
        private const string ValidEmail = "a@b.com";
        private const string ValidPassword = "c";
        private readonly string _existingUserName = TestData.UserA.UserName;
        private readonly string _existingEmail = TestData.UserA.Email;
        
        [Test]
        public void AddUser_WithEmptyUserName_ThrowsValidationError()
        {
            var request = new AddUser.Request("", ValidDisplayName, ValidEmail, ValidPassword, "/");

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void AddUser_WithEmptyDisplayName_ThrowsValidationError()
        {
            var request = new AddUser.Request(ValidUserName, "", ValidEmail, ValidPassword, "/");

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void AddUser_WithEmptyEmail_ThrowsValidationError()
        {
            var request = new AddUser.Request(ValidUserName, ValidDisplayName, "", ValidPassword, "/");

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void AddUser_WithEmptyPAssword_ThrowsValidationError()
        {
            var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, "", "/");

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void AddUser_UserNameAlreadyInUse_ThrowsException()
        {
            var request = new AddUser.Request(_existingUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");

            Assert.Throws<UserExistsException>(() => Sut.Execute(request));
        }

        [Test]
        public void AddUser_EmailAlreadyInUse_ThrowsException()
        {
            var request = new AddUser.Request(ValidUserName, ValidDisplayName, _existingEmail, ValidPassword, "/");

            Assert.Throws<EmailExistsException>(() => Sut.Execute(request));
        }

        [Test]
        public void AddUser_WithValidInput_UserWithCorrectPropertiesIsAdded()
        {
            const string expectedEncryptedPassword = "1cb313748ba4b822b78fe05de42558539efd9156";
            const string expectedSalt = "aaaaaaaaaa";

            var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");
            Sut.Execute(request);

            var user = Repos.User.Added;

            Assert.AreEqual(0, user.Id);
            Assert.AreEqual(ValidUserName, user.UserName);
            Assert.AreEqual(ValidDisplayName, user.DisplayName);
            Assert.AreEqual("", user.RealName);
            Assert.AreEqual(ValidEmail, user.Email);
            Assert.AreEqual(Role.Player, user.GlobalRole);
            Assert.AreEqual(expectedEncryptedPassword, user.EncryptedPassword);
            Assert.AreEqual(expectedSalt, user.Salt);
        }

        [Test]
        public void AddUser_WithValidInput_SendsRegistrationEmail()
        {
            const string subject = "Poker Bunch Registration";
            const string body = @"Thanks for registering with Poker Bunch.

Please sign in here: /loginUrl";

            var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/loginUrl");
            Sut.Execute(request);

            Assert.AreEqual(ValidEmail, Services.MessageSender.To);
            Assert.AreEqual(subject, Services.MessageSender.Message.Subject);
            Assert.AreEqual(body, Services.MessageSender.Message.Body);
        }

        private AddUser Sut => new AddUser(
            Repos.User,
            Services.RandomService,
            Services.MessageSender);
    }
}

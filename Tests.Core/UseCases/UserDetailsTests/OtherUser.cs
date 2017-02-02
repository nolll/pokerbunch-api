using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests
{
    public class OtherUser : Arrange
    {
        [Test]
        public void UserNameIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(ViewUserName, result.UserName);
        }

        [Test]
        public void DisplayNameIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(DisplayName, result.DisplayName);
        }

        [Test]
        public void RealNameIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(RealName, result.RealName);
        }

        [Test]
        public void EmailIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(Email, result.Email);
        }

        [Test]
        public void CanEditIsFalse()
        {
            var result = Sut.Execute(Request);
            Assert.IsFalse(result.CanEdit);
        }

        [Test]
        public void CanChangePasswordIsFalse()
        {
            var result = Sut.Execute(Request);
            Assert.IsFalse(result.CanChangePassword);
        }

        [Test]
        public void AvatarUrlIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual("http://www.gravatar.com/avatar/0c83f57c786a0b4a39efab23731c7ebc?s=100", result.AvatarUrl);
        }
    }
}

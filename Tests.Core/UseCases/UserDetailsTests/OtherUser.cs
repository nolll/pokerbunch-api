using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests;

public class OtherUser : Arrange
{
    [Test]
    public void UserNameIsSet() => Assert.AreEqual(ViewUserName, Result.UserName);

    [Test]
    public void DisplayNameIsSet() => Assert.AreEqual(DisplayName, Result.DisplayName);

    [Test]
    public void RealNameIsSet() => Assert.AreEqual(RealName, Result.RealName);

    [Test]
    public void EmailIsSet() => Assert.AreEqual(Email, Result.Email);

    [Test]
    public void CanViewAllIsFalse() => Assert.IsFalse(Result.CanViewAll);

    [Test]
    public void AvatarUrlIsSet() => Assert.AreEqual("https://gravatar.com/avatar/0c83f57c786a0b4a39efab23731c7ebc?s=100&d=blank", Result.AvatarUrl);
}
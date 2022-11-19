namespace Tests.Core.UseCases.UserDetailsTests;

public class OtherUser : Arrange
{
    [Test]
    public void UserNameIsSet() => Assert.AreEqual(ViewUserName, Result.Data.UserName);

    [Test]
    public void DisplayNameIsSet() => Assert.AreEqual(DisplayName, Result.Data.DisplayName);

    [Test]
    public void RealNameIsSet() => Assert.AreEqual(RealName, Result.Data.RealName);

    [Test]
    public void EmailIsSet() => Assert.AreEqual(Email, Result.Data.Email);

    [Test]
    public void CanViewAllIsFalse() => Assert.IsFalse(Result.Data.CanViewAll);

    [Test]
    public void AvatarUrlIsSet() => Assert.AreEqual("https://gravatar.com/avatar/0c83f57c786a0b4a39efab23731c7ebc?s=100&d=blank", Result.Data.AvatarUrl);
}
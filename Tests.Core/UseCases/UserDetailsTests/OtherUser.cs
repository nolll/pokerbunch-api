namespace Tests.Core.UseCases.UserDetailsTests;

public class OtherUser : Arrange
{
    [Test]
    public void UserNameIsSet() => Assert.That(Result?.Data?.UserName, Is.EqualTo(ViewUserName));

    [Test]
    public void DisplayNameIsSet() => Assert.That(Result?.Data?.DisplayName, Is.EqualTo(DisplayName));

    [Test]
    public void RealNameIsSet() => Assert.That(Result?.Data?.RealName, Is.EqualTo(RealName));

    [Test]
    public void EmailIsSet() => Assert.That(Result?.Data?.Email, Is.EqualTo(Email));

    [Test]
    public void CanViewAllIsFalse() => Assert.That(Result?.Data?.CanViewAll, Is.False);

    [Test]
    public void AvatarUrlIsSet() => Assert.That(Result?.Data?.AvatarUrl, Is.EqualTo("https://gravatar.com/avatar/0c83f57c786a0b4a39efab23731c7ebc?s=100&d=blank"));
}
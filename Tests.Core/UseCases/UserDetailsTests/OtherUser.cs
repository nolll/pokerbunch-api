namespace Tests.Core.UseCases.UserDetailsTests;

public class OtherUser : Arrange
{
    [Test]
    public void UserNameIsSet() => Result?.Data?.UserName.Should().Be(ViewUserName);

    [Test]
    public void DisplayNameIsSet() => Result?.Data?.DisplayName.Should().Be(DisplayName);

    [Test]
    public void RealNameIsSet() => Result?.Data?.RealName.Should().Be(RealName);

    [Test]
    public void EmailIsSet() => Result?.Data?.Email.Should().Be(Email);

    [Test]
    public void CanViewAllIsFalse() => Result?.Data?.CanViewAll.Should().BeFalse();

    [Test]
    public void AvatarUrlIsSet() => 
        Result?.Data?.AvatarUrl.Should().Be("https://gravatar.com/avatar/0c83f57c786a0b4a39efab23731c7ebc?s=100&d=blank");
}
namespace Tests.Core.UseCases.TestEmailTests;

public class WithAdmin : Arrange
{
    protected override bool CanSendTestEmail => true;

    [Test]
    public void MessageIsSent()
    {
        // todo: Move email to config
        To.Should().Be("henriks@gmail.com");
        Subject.Should().Be("Test Email");
        Body.Should().Be("This is a test email from pokerbunch.com");
    }

    [Test]
    public void EmailIsSet() => Result?.Data?.Email.Should().Be(Email);
}
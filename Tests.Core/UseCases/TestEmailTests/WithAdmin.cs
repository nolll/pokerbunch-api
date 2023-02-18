using Core.Entities;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithAdmin : Arrange
{
    protected override Role Role => Role.Admin;

    [Test]
    public void MessageIsSent()
    {
        // todo: Move email to config
        Assert.That(To, Is.EqualTo("henriks@gmail.com"));
        Assert.That(Subject, Is.EqualTo("Test Email"));
        Assert.That(Body, Is.EqualTo("This is a test email from pokerbunch.com"));
    }

    [Test]
    public void EmailIsSet()
    {
        Assert.That(Result?.Data?.Email, Is.EqualTo(Email));
    }
}
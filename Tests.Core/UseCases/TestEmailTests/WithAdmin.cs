using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithAdmin : Arrange
{
    protected override Role Role => Role.Admin;

    [Test]
    public void MessageIsSent()
    {
        Assert.AreEqual("henriks@gmail.com", To);
        Assert.AreEqual("Test Email", Subject);
        Assert.AreEqual("This is a test email from pokerbunch.com", Body);
    }

    [Test]
    public void EmailIsSet()
    {
        Assert.AreEqual(Email, Result.Data.Email);
    }
}
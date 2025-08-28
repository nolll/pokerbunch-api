using Core.Errors;
using Core.Services;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithPlayer : Arrange
{
    protected override IAccessControl AccessControl => AccessControlInTest.Deny;

    [Test]
    public void ReturnsError()
    {
        Assert.That(Result?.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}
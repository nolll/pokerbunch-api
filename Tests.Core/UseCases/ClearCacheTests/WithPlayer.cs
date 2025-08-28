using Core.Errors;
using Core.Services;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests;

public class WithPlayer : Arrange
{
    protected override IAccessControl AccessControl => AccessControlInTest.Deny;

    protected override void Setup()
    {
    }
    
    [Test]
    public void ReturnsError()
    {
        Assert.That(Result?.Success, Is.False);
        Assert.That(Result?.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}
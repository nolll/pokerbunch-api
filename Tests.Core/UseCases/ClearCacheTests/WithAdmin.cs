using Core.Services;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests;

public class WithAdmin : Arrange
{
    protected override IAccessControl AccessControl => AccessControlInTest.Allow;

    protected override void Setup()
    {
    }
    
    [Test]
    public void NoException()
    {
        Execute();
    }
}
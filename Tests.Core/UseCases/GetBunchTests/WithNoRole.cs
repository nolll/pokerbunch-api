using Core.Errors;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithNoRole : Arrange
{
    [Test]
    public void AccessDenied()
    {
        Assert.That(Result?.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}
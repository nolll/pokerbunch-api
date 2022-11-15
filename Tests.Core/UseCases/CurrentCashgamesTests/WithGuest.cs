using Core.Errors;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class WithGuest : Arrange
{
    [Test]
    public void ReturnsError()
    {
        Assert.That(Result.Success, Is.False);
        Assert.That(Result.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}
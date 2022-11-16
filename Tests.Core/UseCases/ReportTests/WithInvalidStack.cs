using Core.Errors;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.ReportTests;

public class WithInvalidStack : Arrange
{
    protected override int Stack => -1;

    [Test]
    public void ReturnsValidationError()
    {
        Assert.That(Result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }
}
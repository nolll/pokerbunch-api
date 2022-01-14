using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.ReportTests;

public class WithInvalidStack : Arrange
{
    protected override bool ExecuteAutomatically => false;
    protected override int Stack => -1;

    [Test]
    public void ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(Execute);
    }
}
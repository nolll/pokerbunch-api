using Core.Errors;
using NUnit.Framework;

namespace Tests.Core.UseCases.CashoutTests;

public class InvalidStack : Arrange
{
    protected override int CashoutStack => -1;

    [Test]
    public void ReturnsError()
    {
        Assert.That(Result.Success, Is.False);
        Assert.That(Result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }
}
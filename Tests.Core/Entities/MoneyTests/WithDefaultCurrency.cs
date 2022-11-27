using Core.Entities;

namespace Tests.Core.Entities.MoneyTests;

public class WithDefaultCurrency : Arrange
{
    [Test]
    public void SmallValue()
    {
        var result = new Money(1, Currency).ToString();
        Assert.That(result, Is.EqualTo("$1"));
    }

    [Test]
    public void NegativeValue()
    {
        var result = new Money(-1, Currency).ToString();
        Assert.That(result, Is.EqualTo("-$1"));
    }

    [Test]
    public void LargeValue()
    {
        // The space is a non breaking space (160)
        var result = new Money(1000, Currency).ToString();
        Assert.That(result, Is.EqualTo("$1 000"));
    }
}
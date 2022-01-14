using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.Entities.MoneyTests;

public class WithSwedishCurrency : Arrange
{
    protected override Currency Currency => new Currency("kr", "{AMOUNT} {SYMBOL}");

    [Test]
    public void SmallValue()
    {
        Assert.AreEqual("1 kr", new Money(1, Currency).ToString());
    }

    [Test]
    public void NegativeValue()
    {
        Assert.AreEqual("-1 kr", new Money(-1, Currency).ToString());
    }

    [Test]
    public void LargeValue()
    {
        // The space is a non breaking space (160)
        Assert.AreEqual("1 000 kr", new Money(1000, Currency).ToString());
    }
}
using Core.Entities;
using NUnit.Framework;

namespace Tests.Core;

class DateTests
{
    private const int Year = 2000;
    private const int Month = 2;
    private const int Day = 3;

    [Test]
    public void Construct_PropertiesAreSet()
    {
        Assert.AreEqual(Year, Sut.Year);
        Assert.AreEqual(Month, Sut.Month);
        Assert.AreEqual(Day, Sut.Day);
    }

    [Test]
    public void IsoString_IsCorrectFormat()
    {
        Assert.AreEqual("2000-02-03", Sut.IsoString);
    }

    private Date Sut => new Date(Year, Month, Day);
}
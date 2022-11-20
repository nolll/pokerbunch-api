using Core.Entities;

namespace Tests.Core;

class DateTests
{
    private const int Year = 2000;
    private const int Month = 2;
    private const int Day = 3;

    [Test]
    public void Construct_PropertiesAreSet()
    {
        Assert.That(Sut.Year, Is.EqualTo(Year));
        Assert.That(Sut.Month, Is.EqualTo(Month));
        Assert.That(Sut.Day, Is.EqualTo(Day));
    }

    [Test]
    public void IsoString_IsCorrectFormat()
    {
        Assert.That(Sut.IsoString, Is.EqualTo("2000-02-03"));
    }

    private static Date Sut => new(Year, Month, Day);
}
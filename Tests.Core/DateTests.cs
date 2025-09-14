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
        Sut.Year.Should().Be(Year);
        Sut.Month.Should().Be(Month);
        Sut.Day.Should().Be(Day);
    }

    [Test]
    public void IsoString_IsCorrectFormat() => Sut.IsoString.Should().Be("2000-02-03");

    private static Date Sut => new(Year, Month, Day);
}
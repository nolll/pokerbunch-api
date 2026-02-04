using System.Globalization;
using Core.Services;

namespace Tests.Core.Services;

public class DateTimeServiceTests
{
    [Fact]
    public void ConvertsFromUnixTimestamp()
    {
        var result = DateTimeService.UtcFromUnixTimeStamp(1_750_000_000);
        result.ToString(CultureInfo.CreateSpecificCulture("sv-SE")).Should().Be("2025-06-15 15:06:40");
    }
}
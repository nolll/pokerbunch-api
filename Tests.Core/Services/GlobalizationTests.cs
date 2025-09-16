using System;
using Core.Services;

namespace Tests.Core.Services;

public class GlobalizationTests
{
    [Fact]
    public void FormatIsoDate() => 
        Globalization.FormatIsoDate(DateTime.Parse("2010-02-01 12:28:35")).Should().Be("2010-02-01");
}
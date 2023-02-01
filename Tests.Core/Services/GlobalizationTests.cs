using System;
using Core.Services;

namespace Tests.Core.Services;

public class GlobalizationTests
{
    [Test]
    public void FormatIsoDate()
    {
        var dateTime = DateTime.Parse("2010-02-01 12:28:35");
        const string expected = "2010-02-01";
            
        var result = Globalization.FormatIsoDate(dateTime);
        Assert.That(result, Is.EqualTo(expected));
    }
}
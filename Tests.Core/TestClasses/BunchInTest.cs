using System;
using Core.Entities;

namespace Tests.Core.TestClasses;

public class BunchInTest : Bunch
{
    public BunchInTest(
        string? id = null, 
        string? slug = null, 
        string? displayName = null, 
        string? description = null, 
        string? houseRules = null, 
        TimeZoneInfo? timezone = null, 
        int defaultBuyin = 0, 
        Currency? currency = null)
        : base(
            id ?? "",
            slug ?? "",
            displayName,
            description,
            houseRules, 
            timezone, 
            defaultBuyin, 
            currency)
    {
    }
}
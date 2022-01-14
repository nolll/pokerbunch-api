using System;

namespace Core.Entities;

public class Bunch : IEntity
{
    public int Id { get; }
    public string Slug { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public string HouseRules { get; }
    public TimeZoneInfo Timezone { get; }
    public int DefaultBuyin { get; }
    public Currency Currency { get; }
    public bool CashgamesEnabled { get; }
    public bool TournamentsEnabled { get; }
    public bool VideosEnabled { get; }

    public Bunch(
        int id, 
        string slug, 
        string displayName = null, 
        string description = null, 
        string houseRules = null, 
        TimeZoneInfo timezone = null, 
        int defaultBuyin = 0, 
        Currency currency = null)
    {
        Id = id;
        Slug = slug;
        DisplayName = displayName ?? string.Empty;
        Description = description ?? string.Empty;
        HouseRules = houseRules ?? string.Empty;
        Timezone = timezone ?? TimeZoneInfo.Utc;
        DefaultBuyin = defaultBuyin;
        Currency = currency ?? Currency.Default;
        CashgamesEnabled = true;
        TournamentsEnabled = false;
        VideosEnabled = false;
    }
}
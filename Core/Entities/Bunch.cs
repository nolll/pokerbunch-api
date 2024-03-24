using System;

namespace Core.Entities;

public class Bunch(
    string id,
    string slug,
    string? displayName = null,
    string? description = null,
    string? houseRules = null,
    TimeZoneInfo? timezone = null,
    int? defaultBuyin = null,
    Currency? currency = null)
    : IEntity
{
    public string Id { get; } = id;
    public string Slug { get; } = slug;
    public string DisplayName { get; } = displayName ?? "";
    public string Description { get; } = description ?? "";
    public string HouseRules { get; } = houseRules ?? "";
    public TimeZoneInfo Timezone { get; } = timezone ?? TimeZoneInfo.Utc;
    public int DefaultBuyin { get; } = defaultBuyin ?? 0;
    public Currency Currency { get; } = currency ?? Currency.Default;
    public bool CashgamesEnabled { get; } = true;
    public bool TournamentsEnabled { get; } = false;
    public bool VideosEnabled { get; } = false;
}
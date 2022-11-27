using Core.Entities;

namespace Infrastructure.Sql.Classes;

public class RawBunch
{
    public string Id { get; }
    public string Slug { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public string HouseRules { get; }
    public string TimezoneName { get; }
    public int DefaultBuyin { get; }
    public string CurrencyLayout { get; }
    public string CurrencySymbol { get; }
    public bool CashgamesEnabled { get; }
    public bool TournamentsEnabled { get; }
    public bool VideosEnabled { get; }

    public RawBunch(string id, string slug, string displayName, string description, string houseRules, string timezoneName, int defaultBuyin, string currencyLayout, string currencySymbol, bool cashgamesEnabled, bool tournamentsEnabled, bool videosEnabled)
    {
        Id = id;
        Slug = slug;
        DisplayName = displayName;
        Description = description;
        HouseRules = houseRules;
        TimezoneName = timezoneName;
        DefaultBuyin = defaultBuyin;
        CurrencyLayout = currencyLayout;
        CurrencySymbol = currencySymbol;
        CashgamesEnabled = cashgamesEnabled;
        TournamentsEnabled = tournamentsEnabled;
        VideosEnabled = videosEnabled;
    }

    public static RawBunch Create(Bunch bunch)
    {
        return new RawBunch(
            bunch.Id,
            bunch.Slug,
            bunch.DisplayName,
            bunch.Description,
            bunch.HouseRules,
            bunch.Timezone.Id,
            bunch.DefaultBuyin,
            bunch.Currency.Layout,
            bunch.Currency.Symbol,
            bunch.CashgamesEnabled,
            bunch.TournamentsEnabled,
            bunch.VideosEnabled);
    }
}
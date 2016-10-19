using Core.Entities;

namespace Infrastructure.Sql.Classes
{
    public class RawBunch
    {
        public int Id { get; private set; }
        public string Slug { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public string HouseRules { get; private set; }
        public string TimezoneName { get; private set; }
        public int DefaultBuyin { get; private set; }
        public string CurrencyLayout { get; private set; }
        public string CurrencySymbol { get; private set; }
        public bool CashgamesEnabled { get; private set; }
        public bool TournamentsEnabled { get; private set; }
        public bool VideosEnabled { get; private set; }

        public RawBunch(int id, string slug, string displayName, string description, string houseRules, string timezoneName, int defaultBuyin, string currencyLayout, string currencySymbol, bool cashgamesEnabled, bool tournamentsEnabled, bool videosEnabled)
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
}

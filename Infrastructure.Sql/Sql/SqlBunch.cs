namespace Infrastructure.Sql.Sql;

public class SqlBunch : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn Name { get; }
    public SqlColumn DisplayName { get; }
    public SqlColumn Description { get; }
    public SqlColumn Currency { get; }
    public SqlColumn CurrencyLayout { get; }
    public SqlColumn Timezone { get; }
    public SqlColumn DefaultBuyin { get; }
    public SqlColumn CashgamesEnabled { get; }
    public SqlColumn TournamentsEnabled { get; }
    public SqlColumn VideosEnabled { get; }
    public SqlColumn HouseRules { get; }

    public SqlBunch() : base("pb_bunch")
    {
        Id = new SqlColumn(this, "bunch_id");
        Name = new SqlColumn(this, "name");
        DisplayName = new SqlColumn(this, "display_name");
        Description = new SqlColumn(this, "description");
        Currency = new SqlColumn(this, "currency");
        CurrencyLayout = new SqlColumn(this, "currency_layout");
        Timezone = new SqlColumn(this, "timezone");
        DefaultBuyin = new SqlColumn(this, "default_buyin");
        CashgamesEnabled = new SqlColumn(this, "cashgames_enabled");
        TournamentsEnabled = new SqlColumn(this, "tournaments_enabled");
        VideosEnabled = new SqlColumn(this, "videos_enabled");
        HouseRules = new SqlColumn(this, "house_rules");
    }
}
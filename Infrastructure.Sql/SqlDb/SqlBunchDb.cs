using System.Globalization;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql.SqlDb;

public class SqlBunchDb
{
    private const string DataSql = @"
        SELECT b.bunch_id, b.name, b.display_name, b.description, b.currency, b.currency_layout, b.timezone, b.default_buyin, b.cashgames_enabled, b.tournaments_enabled, b.videos_enabled, b.house_rules
        FROM pb_bunch b";

    private const string SearchSql = @"
        SELECT b.bunch_id
        FROM pb_bunch b";

    private readonly PostgresStorageProvider _db;
        
    public SqlBunchDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var sql = string.Concat(DataSql, " WHERE b.bunch_id IN(@ids)");
        var parameter = new ListSqlParameter("@ids", ids.Select(int.Parse).ToList());
        var reader = await _db.QueryAsync(sql, parameter);
        var rawHomegames = reader.ReadList(CreateRawBunch);
        return rawHomegames.Select(CreateBunch).ToList();
    }

    public async Task<Bunch> Get(string id)
    {
        var sql = string.Concat(DataSql, " WHERE bunch_id = @id");
        var parameters = new List<SimpleSqlParameter>
        {
            new IntSqlParameter("@id", id)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        var rawHomegame = reader.ReadOne(CreateRawBunch);
        return rawHomegame != null ? CreateBunch(rawHomegame) : null;
    }

    public async Task<IList<string>> Search()
    {
        var reader = await _db.QueryAsync(SearchSql);
        return reader.ReadIntList("bunch_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var sql = string.Concat(SearchSql, " WHERE b.name = @slug");
        var parameters = new List<SimpleSqlParameter>
        {
            new StringSqlParameter("@slug", slug)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        var id = reader.ReadInt("bunch_id")?.ToString();
        if(id != null)
            return new List<string> {id};
        return new List<string>();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var sql = string.Concat(SearchSql, " INNER JOIN pb_player p on b.bunch_id = p.bunch_id WHERE p.user_id = @userId ORDER BY b.name");
        var parameters = new List<SimpleSqlParameter>
        {
            new IntSqlParameter("@userId", userId)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        return reader.ReadIntList("bunch_id").Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);
        const string sql = @"
            INSERT INTO pb_bunch (name, display_name, description, currency, currency_layout, timezone, default_buyin, cashgames_enabled, tournaments_enabled, videos_enabled, house_rules)
            VALUES (@slug, @displayName, @description, @currencySymbol, @currencyLayout, @timeZone, 0, @cashgamesEnabled, @tournamentsEnabled, @videosEnabled, @houseRules) RETURNING bunch_id";

        var parameters = new List<SimpleSqlParameter>
        {
            new StringSqlParameter("@slug", rawBunch.Slug),
            new StringSqlParameter("@displayName", rawBunch.DisplayName),
            new StringSqlParameter("@description", rawBunch.Description),
            new StringSqlParameter("@currencySymbol", rawBunch.CurrencySymbol),
            new StringSqlParameter("@currencyLayout", rawBunch.CurrencyLayout),
            new StringSqlParameter("@timeZone", rawBunch.TimezoneName),
            new BooleanSqlParameter("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new BooleanSqlParameter("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new BooleanSqlParameter("@videosEnabled", rawBunch.VideosEnabled),
            new StringSqlParameter("@houseRules", rawBunch.HouseRules)
        };
        return (await _db.ExecuteInsertAsync(sql, parameters)).ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);
        const string sql = @"
            UPDATE pb_bunch
            SET name = @slug,
                display_name = @displayName,
                description = @description, 
                house_rules = @houseRules,
                currency = @currencySymbol,
                currency_layout = @currencyLayout,
                timezone = @timeZone,
                default_buyin = @defaultBuyin,
                cashgames_enabled = @cashgamesEnabled,
                tournaments_enabled = @tournamentsEnabled,
                videos_enabled = @videosEnabled
            WHERE bunch_id = @id";

        var parameters = new List<SimpleSqlParameter>
        {
            new StringSqlParameter("@slug", rawBunch.Slug),
            new StringSqlParameter("@displayName", rawBunch.DisplayName),
            new StringSqlParameter("@description", rawBunch.Description),
            new StringSqlParameter("@houseRules", rawBunch.HouseRules),
            new StringSqlParameter("@currencySymbol", rawBunch.CurrencySymbol),
            new StringSqlParameter("@currencyLayout", rawBunch.CurrencyLayout),
            new StringSqlParameter("@timeZone", rawBunch.TimezoneName),
            new IntSqlParameter("@defaultBuyin", rawBunch.DefaultBuyin),
            new BooleanSqlParameter("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new BooleanSqlParameter("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new BooleanSqlParameter("@videosEnabled", rawBunch.VideosEnabled),
            new IntSqlParameter("@id", rawBunch.Id)
        };

        await _db.ExecuteAsync(sql, parameters);
    }
        
    private static Bunch CreateBunch(RawBunch rawBunch)
    {
        var culture = CultureInfo.CreateSpecificCulture("sv-SE");
        var currency = new Currency(rawBunch.CurrencySymbol, rawBunch.CurrencyLayout, culture);

        return new Bunch(
            rawBunch.Id,
            rawBunch.Slug,
            rawBunch.DisplayName,
            rawBunch.Description,
            rawBunch.HouseRules,
            TimeZoneInfo.FindSystemTimeZoneById(rawBunch.TimezoneName),
            rawBunch.DefaultBuyin,
            currency);
    }
        
    public bool DeleteBunch(string id)
    {
        const string sql = @"
            DELETE
            FROM pb_bunch
            WHERE bunch_id = @id";

        var parameters = new List<SimpleSqlParameter>
        {
            new IntSqlParameter("@id", id)
        };
        var rowCount = _db.Execute(sql, parameters);
        return rowCount > 0;
    }

    private static RawBunch CreateRawBunch(IStorageDataReader reader)
    {
        return new RawBunch(
            reader.GetIntValue("bunch_id").ToString(),
            reader.GetStringValue("name"),
            reader.GetStringValue("display_name"),
            reader.GetStringValue("description"),
            reader.GetStringValue("house_rules"),
            reader.GetStringValue("timezone"),
            reader.GetIntValue("default_buyin"),
            reader.GetStringValue("currency_layout"),
            reader.GetStringValue("currency"),
            reader.GetBooleanValue("cashgames_enabled"),
            reader.GetBooleanValue("tournaments_enabled"),
            reader.GetBooleanValue("videos_enabled"));
    }
}
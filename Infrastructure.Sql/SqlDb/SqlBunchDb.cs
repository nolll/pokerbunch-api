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

    private readonly IDb _db;
        
    public SqlBunchDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var sql = string.Concat(DataSql, " WHERE b.bunch_id IN(@ids)");
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.Query(sql, parameter);
        var rawBunches = reader.ReadList(CreateRawBunch);
        return rawBunches.Select(CreateBunch).ToList();
    }

    public async Task<Bunch> Get(string id)
    {
        var sql = string.Concat(DataSql, " WHERE bunch_id = @id");
        var parameters = new List<SqlParam>
        {
            new IntParam("@id", id)
        };
        var reader = await _db.Query(sql, parameters);
        var rawBunch = reader.ReadOne(CreateRawBunch);
        return rawBunch != null ? CreateBunch(rawBunch) : null;
    }

    public async Task<IList<string>> Search()
    {
        var reader = await _db.Query(SearchSql);
        return reader.ReadIntList("bunch_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var sql = string.Concat(SearchSql, " WHERE b.name = @slug");
        var parameters = new List<SqlParam>
        {
            new StringParam("@slug", slug)
        };
        var reader = await _db.Query(sql, parameters);
        var id = reader.ReadInt("bunch_id")?.ToString();
        if(id != null)
            return new List<string> {id};
        return new List<string>();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var sql = string.Concat(SearchSql, " INNER JOIN pb_player p on b.bunch_id = p.bunch_id WHERE p.user_id = @userId ORDER BY b.name");
        var parameters = new List<SqlParam>
        {
            new IntParam("@userId", userId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("bunch_id").Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);
        const string sql = @"
            INSERT INTO pb_bunch (name, display_name, description, currency, currency_layout, timezone, default_buyin, cashgames_enabled, tournaments_enabled, videos_enabled, house_rules)
            VALUES (@slug, @displayName, @description, @currencySymbol, @currencyLayout, @timeZone, 0, @cashgamesEnabled, @tournamentsEnabled, @videosEnabled, @houseRules) RETURNING bunch_id";

        var parameters = new List<SqlParam>
        {
            new StringParam("@slug", rawBunch.Slug),
            new StringParam("@displayName", rawBunch.DisplayName),
            new StringParam("@description", rawBunch.Description),
            new StringParam("@currencySymbol", rawBunch.CurrencySymbol),
            new StringParam("@currencyLayout", rawBunch.CurrencyLayout),
            new StringParam("@timeZone", rawBunch.TimezoneName),
            new BoolParam("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new BoolParam("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new BoolParam("@videosEnabled", rawBunch.VideosEnabled),
            new StringParam("@houseRules", rawBunch.HouseRules)
        };
        return (await _db.Insert(sql, parameters)).ToString();
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

        var parameters = new List<SqlParam>
        {
            new StringParam("@slug", rawBunch.Slug),
            new StringParam("@displayName", rawBunch.DisplayName),
            new StringParam("@description", rawBunch.Description),
            new StringParam("@houseRules", rawBunch.HouseRules),
            new StringParam("@currencySymbol", rawBunch.CurrencySymbol),
            new StringParam("@currencyLayout", rawBunch.CurrencyLayout),
            new StringParam("@timeZone", rawBunch.TimezoneName),
            new IntParam("@defaultBuyin", rawBunch.DefaultBuyin),
            new BoolParam("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new BoolParam("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new BoolParam("@videosEnabled", rawBunch.VideosEnabled),
            new IntParam("@id", rawBunch.Id)
        };

        await _db.Execute(sql, parameters);
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
        
    public async Task<bool> DeleteBunch(string id)
    {
        const string sql = @"
            DELETE
            FROM pb_bunch
            WHERE bunch_id = @id";

        var parameters = new List<SqlParam>
        {
            new IntParam("@id", id)
        };
        var rowCount = await _db.Execute(sql, parameters);
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
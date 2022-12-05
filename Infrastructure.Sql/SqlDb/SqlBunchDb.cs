using System;
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
        
        var @params = new
        {
            id = int.Parse(id)
        };
        var rawBunch = await _db.Single<RawBunch>(sql, @params);
        return rawBunch != null ? CreateBunch(rawBunch) : null;
    }

    public async Task<IList<string>> Search()
    {
        return (await _db.List<int>(SearchSql)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var sql = string.Concat(SearchSql, " WHERE b.name = @slug");
        
        var @params = new
        {
            slug = slug
        };
        
        var id = (await _db.Single<int?>(sql, @params))?.ToString();
        return id != null 
            ? new List<string> {id} 
            : new List<string>();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var sql = string.Concat(SearchSql, " INNER JOIN pb_player p on b.bunch_id = p.bunch_id WHERE p.user_id = @userId ORDER BY b.name");

        var @params = new
        {
            userId = int.Parse(userId)
        };

        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);
        const string sql = @"
            INSERT INTO pb_bunch (name, display_name, description, currency, currency_layout, timezone, default_buyin, cashgames_enabled, tournaments_enabled, videos_enabled, house_rules)
            VALUES (@slug, @displayName, @description, @currencySymbol, @currencyLayout, @timeZone, 0, @cashgamesEnabled, @tournamentsEnabled, @videosEnabled, @houseRules) RETURNING bunch_id";

        var @params = new
        {
            slug = rawBunch.Name,
            displayName = rawBunch.Display_Name,
            description = rawBunch.Description,
            currencySymbol = rawBunch.Currency,
            currencyLayout = rawBunch.Currency_Layout,
            timeZone = rawBunch.Timezone,
            cashgamesEnabled = rawBunch.Cashgames_Enabled,
            tournamentsEnabled = rawBunch.Tournaments_Enabled,
            videosEnabled = rawBunch.Videos_Enabled,
            @houseRules = rawBunch.House_Rules
        };

        return (await _db.Insert(sql, @params)).ToString();
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

        var @params = new
        {
            slug = rawBunch.Name,
            displayName = rawBunch.Display_Name,
            description = rawBunch.Description,
            houseRules = rawBunch.House_Rules,
            currencySymbol = rawBunch.Currency,
            currencyLayout = rawBunch.Currency_Layout,
            timeZone = rawBunch.Timezone,
            defaultBuyin = rawBunch.Default_Buyin,
            cashgamesEnabled = rawBunch.Cashgames_Enabled,
            tournamentsEnabled = rawBunch.Tournaments_Enabled,
            videosEnabled = rawBunch.Videos_Enabled,
            id = int.Parse(rawBunch.Bunch_Id)
        };

        await _db.Execute(sql, @params);
    }
        
    private static Bunch CreateBunch(RawBunch rawBunch)
    {
        var culture = CultureInfo.CreateSpecificCulture("sv-SE");
        var currency = new Currency(rawBunch.Currency, rawBunch.Currency_Layout, culture);

        return new Bunch(
            rawBunch.Bunch_Id,
            rawBunch.Name,
            rawBunch.Display_Name,
            rawBunch.Description,
            rawBunch.House_Rules,
            TimeZoneInfo.FindSystemTimeZoneById(rawBunch.Timezone),
            rawBunch.Default_Buyin,
            currency);
    }
        
    public async Task<bool> DeleteBunch(string id)
    {
        const string sql = @"
            DELETE
            FROM pb_bunch
            WHERE bunch_id = @id";

        var @params = new
        {
            id = int.Parse(id)
        };

        var rowCount = await _db.Execute(sql, @params);
        return rowCount > 0;
    }

    private static RawBunch CreateRawBunch(IStorageDataReader reader)
    {
        return new RawBunch
        {
            Bunch_Id = reader.GetIntValue("bunch_id").ToString(),
            Name = reader.GetStringValue("name"),
            Display_Name = reader.GetStringValue("display_name"),
            Description = reader.GetStringValue("description"),
            House_Rules = reader.GetStringValue("house_rules"),
            Timezone = reader.GetStringValue("timezone"),
            Default_Buyin = reader.GetIntValue("default_buyin"),
            Currency_Layout = reader.GetStringValue("currency_layout"),
            Currency = reader.GetStringValue("currency"),
            Cashgames_Enabled = reader.GetBooleanValue("cashgames_enabled"),
            Tournaments_Enabled = reader.GetBooleanValue("tournaments_enabled"),
            Videos_Enabled = reader.GetBooleanValue("videos_enabled")
        };
    }
}
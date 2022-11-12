using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

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

    public IList<Bunch> Get(IList<int> ids)
    {
        var sql = string.Concat(DataSql, " WHERE b.bunch_id IN(@ids)");
        var parameter = new ListSqlParameter("@ids", ids);
        var reader = _db.Query(sql, parameter);
        var rawHomegames = reader.ReadList(CreateRawBunch);
        return rawHomegames.Select(CreateBunch).ToList();
    }

    public Bunch Get(int id)
    {
        var sql = string.Concat(DataSql, " WHERE bunch_id = @id");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", id)
        };
        var reader = _db.Query(sql, parameters);
        var rawHomegame = reader.ReadOne(CreateRawBunch);
        return rawHomegame != null ? CreateBunch(rawHomegame) : null;
    }

    public IList<int> Search()
    {
        var reader = _db.Query(SearchSql);
        return reader.ReadIntList("bunch_id");
    }

    public IList<int> Search(string slug)
    {
        var sql = string.Concat(SearchSql, " WHERE b.name = @slug");
        var parameters = new SqlParameters(new SimpleSqlParameter("@slug", slug));
        var reader = _db.Query(sql, parameters);
        var id = reader.ReadInt("bunch_id");
        if(id.HasValue)
            return new List<int>{id.Value};
        return new List<int>();
    }

    public IList<int> Search(int userId)
    {
        var sql = string.Concat(SearchSql, " INNER JOIN pb_player p on b.bunch_id = p.bunch_id WHERE p.user_id = @userId ORDER BY b.name");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@userId", userId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("bunch_id");
    }
        
    public int Add(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);
        const string sql = @"
            INSERT INTO pb_bunch (name, display_name, description, currency, currency_layout, timezone, default_buyin, cashgames_enabled, tournaments_enabled, videos_enabled, house_rules)
            VALUES (@slug, @displayName, @description, @currencySymbol, @currencyLayout, @timeZone, 0, @cashgamesEnabled, @tournamentsEnabled, @videosEnabled, @houseRules) RETURNING bunch_id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@slug", rawBunch.Slug),
            new("@displayName", rawBunch.DisplayName),
            new("@description", rawBunch.Description),
            new("@currencySymbol", rawBunch.CurrencySymbol),
            new("@currencyLayout", rawBunch.CurrencyLayout),
            new("@timeZone", rawBunch.TimezoneName),
            new("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new("@videosEnabled", rawBunch.VideosEnabled),
            new("@houseRules", rawBunch.HouseRules)
        };
        return _db.ExecuteInsert(sql, parameters);
    }

    public void Update(Bunch bunch)
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
            new("@slug", rawBunch.Slug),
            new("@displayName", rawBunch.DisplayName),
            new("@description", rawBunch.Description),
            new("@houseRules", rawBunch.HouseRules),
            new("@currencySymbol", rawBunch.CurrencySymbol),
            new("@currencyLayout", rawBunch.CurrencyLayout),
            new("@timeZone", rawBunch.TimezoneName),
            new("@defaultBuyin", rawBunch.DefaultBuyin),
            new("@cashgamesEnabled", rawBunch.CashgamesEnabled),
            new("@tournamentsEnabled", rawBunch.TournamentsEnabled),
            new("@videosEnabled", rawBunch.VideosEnabled),
            new("@id", rawBunch.Id)
        };

        _db.Execute(sql, parameters);
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
        
    public bool DeleteBunch(int id)
    {
        const string sql = @"
            DELETE
            FROM pb_bunch
            WHERE bunch_id = @id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", id)
        };
        var rowCount = _db.Execute(sql, parameters);
        return rowCount > 0;
    }

    private static RawBunch CreateRawBunch(IStorageDataReader reader)
    {
        return new RawBunch(
            reader.GetIntValue("bunch_id"),
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
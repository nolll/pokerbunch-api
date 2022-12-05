using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql.SqlDb;

public class SqlEventDb
{
    private const string EventSql = @"
        SELECT e.event_id, e.bunch_id, e.name, g.location_id, g.date
        FROM pb_event e
        LEFT JOIN pb_event_cashgame ecg on e.event_id = ecg.event_id
        LEFT JOIN pb_cashgame g on ecg.cashgame_id = g.cashgame_id
        {0}
        ORDER BY e.event_id, g.date";

    private readonly IDb _db;

    public SqlEventDb(IDb db)
    {
        _db = db;
    }

    public async Task<Event> Get(string id)
    {
        const string whereClause = "WHERE e.event_id = @cashgameId";
        var sql = string.Format(EventSql, whereClause);
        var parameters = new List<SqlParam>
        {
            new IntParam("@cashgameId", id)
        };
        var reader = await _db.Query(sql, parameters);
        var rawEvents = CreateRawEvents(reader);
        var rawEvent = rawEvents.FirstOrDefault();
        return rawEvent != null ? CreateEvent(rawEvent) : null;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        const string whereClause = "WHERE e.event_id IN(@ids)";
        var sql = string.Format(EventSql, whereClause);
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.Query(sql, parameter);
        var rawEvents = CreateRawEvents(reader);
        return rawEvents.Select(CreateEvent).ToList();
    }

    public async Task<IList<string>> FindByBunchId(string bunchId)
    {
        const string sql = @"
            SELECT e.event_id
            FROM pb_event e
            WHERE e.bunch_id = @id";

        var parameters = new List<SqlParam>
        {
            new IntParam("@id", bunchId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("event_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        const string sql = @"
            SELECT ecg.event_id
            FROM pb_event_cashgame ecg
            WHERE ecg.cashgame_id = @id";

        var parameters = new List<SqlParam>
        {
            new IntParam("@id", cashgameId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("event_id").Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        const string sql = @"
            INSERT INTO pb_event (name, bunch_id)
            VALUES (@name, @bunchId) RETURNING event_id";

        var @params = new
        {
            name = e.Name,
            bunchId = int.Parse(e.BunchId)
        };

        return (await _db.Insert(sql, @params)).ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        const string sql = @"
            INSERT INTO pb_event_cashgame (event_id, cashgame_id)
            VALUES (@eventId, @cashgameId)";

        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Insert(sql, @params);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        const string sql = @"
            DELETE FROM pb_event_cashgame
            WHERE event_id = @eventId
            AND cashgame_id = @cashgameId";

        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Execute(sql, @params);
    }

    private static Event CreateEvent(RawEvent rawEvent)
    {
        return new Event(
            rawEvent.Id,
            rawEvent.BunchId,
            rawEvent.Name,
            rawEvent.LocationId,
            new Date(rawEvent.StartDate),
            new Date(rawEvent.EndDate));
    }

    private static IList<RawEvent> CreateRawEvents(IStorageDataReader reader)
    {
        var rawEventDays = reader.ReadList(CreateRawEventDay);
        var map = new Dictionary<string, IList<RawEventDay>>();
        foreach (var day in rawEventDays)
        {
            IList<RawEventDay> list;
            if (map.ContainsKey(day.Id))
            {
                list = map[day.Id];
            }
            else
            {
                list = new List<RawEventDay>();
                map[day.Id] = list;
            }
            list.Add(day);
        }

        var rawEvents = new List<RawEvent>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            rawEvents.Add(new RawEvent(firstItem.Id, firstItem.BunchId, firstItem.Name, firstItem.LocationId, firstItem.Date, lastItem.Date));
        }
        return rawEvents;
    }

    private static RawEventDay CreateRawEventDay(IStorageDataReader reader)
    {
        return new RawEventDay(
            reader.GetIntValue("event_id").ToString(),
            reader.GetIntValue("bunch_id").ToString(),
            reader.GetStringValue("name"),
            reader.GetIntValue("location_id").ToString(),
            reader.GetDateTimeValue("date"));
    }
}
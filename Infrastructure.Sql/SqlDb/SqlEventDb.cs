using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb;

public class SqlEventDb
{
    private const string EventSql = @"
SELECT e.event_id, e.bunch_id, e.name, g.location_id, g.date
FROM pb_event e
LEFT JOIN pb_event_cashgame ecg on e.event_id = ecg.event_id
LEFT JOIN pb_game g on ecg.game_id = g.game_id
{0}
ORDER BY e.event_id, g.date";

    private readonly PostgresStorageProvider _db;

    public SqlEventDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public Event Get(int id)
    {
        const string whereClause = "WHERE e.event_id = @id";
        var sql = string.Format(EventSql, whereClause);
        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", id)
        };
        var reader = _db.Query(sql, parameters);
        var rawEvents = CreateRawEvents(reader);
        var rawEvent = rawEvents.FirstOrDefault();
        return rawEvent != null ? CreateEvent(rawEvent) : null;
    }

    public IList<Event> Get(IList<int> ids)
    {
        const string whereClause = "WHERE e.event_id IN(@ids)";
        var sql = string.Format(EventSql, whereClause);
        var parameter = new ListSqlParameter("@ids", ids);
        var reader = _db.Query(sql, parameter);
        var rawEvents = CreateRawEvents(reader);
        return rawEvents.Select(CreateEvent).ToList();
    }

    public IList<int> FindByBunchId(int bunchId)
    {
        const string sql = @"
SELECT e.event_id
FROM pb_event e
WHERE e.bunch_id = @id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", bunchId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("event_id");
    }

    public IList<int> FindByCashgameId(int cashgameId)
    {
        const string sql = @"
SELECT ecg.event_id
FROM pb_event_cashgame ecg
WHERE ecg.cashgame_id = @id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", cashgameId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("event_id");
    }

    public int Add(Event e)
    {
        const string sql = @"
INSERT INTO pb_event (name, bunch_id)
VALUES (@name, @bunchId) RETURNING event_id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@name", e.Name),
            new("@bunchId", e.BunchId)
        };
        return _db.ExecuteInsert(sql, parameters);
    }

    public void AddCashgame(int eventId, int cashgameId)
    {
        const string sql = @"
INSERT INTO pb_event_cashgame (event_id, game_id)
VALUES (@eventId, @cashgameId)";
        var parameters = new List<SimpleSqlParameter>
        {
            new("@eventId", eventId),
            new("@cashgameId", cashgameId)
        };
        _db.ExecuteInsert(sql, parameters);
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
        var map = new Dictionary<int, IList<RawEventDay>>();
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
            reader.GetIntValue("event_id"),
            reader.GetIntValue("bunch_id"),
            reader.GetStringValue("name"),
            reader.GetIntValue("location_id"),
            reader.GetDateTimeValue("date"));
    }
}
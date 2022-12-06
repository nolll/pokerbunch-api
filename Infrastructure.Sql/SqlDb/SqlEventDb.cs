using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql.SqlDb;

public class SqlEventDb
{
    private const string EventSql = """
        SELECT e.event_id, e.bunch_id, e.name, c.location_id, cc.timestamp
        FROM pb_event e
        LEFT JOIN pb_event_cashgame ec
	        ON ec.event_id = e.event_id
        LEFT JOIN pb_cashgame c
	        ON ec.cashgame_id = c.cashgame_id
        LEFT JOIN pb_cashgame_checkpoint cc
	        ON cc.checkpoint_id = (
		        SELECT checkpoint_id
		        FROM pb_cashgame_checkpoint cc
		        WHERE cashgame_id = c.cashgame_id
		        ORDER BY cc.timestamp DESC
		        LIMIT 1
	        ) 
        {0}
        ORDER BY e.event_id, c.date
""";

    private readonly IDb _db;

    public SqlEventDb(IDb db)
    {
        _db = db;
    }

    public async Task<Event> Get(string id)
    {
        const string whereClause = "WHERE e.event_id = @eventId";
        var sql = string.Format(EventSql, whereClause);
        
        var @params = new
        {
            eventId = int.Parse(id)
        };
        
        var rawEventDays = await _db.List<RawEventDay>(sql, @params);
        var rawEvents = CreateRawEvents(rawEventDays);
        var rawEvent = rawEvents.FirstOrDefault();
        return rawEvent != null ? CreateEvent(rawEvent) : null;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var sql = string.Format(EventSql, "WHERE e.event_id IN (@ids)");
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var rawEventDays = await _db.List<RawEventDay>(sql, param);
        var rawEvents = CreateRawEvents(rawEventDays);
        return rawEvents.Select(CreateEvent).ToList();
    }

    public async Task<IList<string>> FindByBunchId(string bunchId)
    {
        const string sql = @"
            SELECT e.event_id
            FROM pb_event e
            WHERE e.bunch_id = @id";

        var @params = new
        {
            id = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        const string sql = @"
            SELECT ecg.event_id
            FROM pb_event_cashgame ecg
            WHERE ecg.cashgame_id = @id";

        var @params = new
        {
            id = int.Parse(cashgameId)
        };

        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
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
            rawEvent.Event_Id,
            rawEvent.Bunch_Id,
            rawEvent.Name,
            rawEvent.Location_Id,
            new Date(rawEvent.StartDate),
            new Date(rawEvent.EndDate));
    }

    private static IList<RawEvent> CreateRawEvents(IEnumerable<RawEventDay> rawEventDays)
    {
        var map = new Dictionary<string, IList<RawEventDay>>();
        foreach (var day in rawEventDays)
        {
            IList<RawEventDay> list;
            if (map.ContainsKey(day.Event_Id))
            {
                list = map[day.Event_Id];
            }
            else
            {
                list = new List<RawEventDay>();
                map[day.Event_Id] = list;
            }
            list.Add(day);
        }

        var rawEvents = new List<RawEvent>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            rawEvents.Add(new RawEvent(firstItem.Event_Id, firstItem.Bunch_Id, firstItem.Name, firstItem.Location_Id, firstItem.Timestamp, lastItem.Timestamp));
        }
        return rawEvents;
    }
}
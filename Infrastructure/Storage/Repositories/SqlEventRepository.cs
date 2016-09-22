using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Storage.Classes;
using Infrastructure.Storage.Interfaces;

namespace Infrastructure.Storage.Repositories
{
    public class SqlEventRepository : IEventRepository
    {
        private const string EventSql = @"SELECT e.EventID, e.BunchID, e.Name, g.LocationId, g.Date
                                        FROM [Event] e
                                        LEFT JOIN EventCashgame ecg on e.EventId = ecg.EventId
                                        LEFT JOIN Game g on ecg.GameId = g.GameID
                                        {0}
                                        ORDER BY e.EventId, g.Date";

        private readonly SqlServerStorageProvider _db;

        public SqlEventRepository(SqlServerStorageProvider db)
        {
            _db = db;
        }

        public Event Get(int id)
        {
            const string whereClause = "WHERE e.EventID = @id";
            var sql = string.Format(EventSql, whereClause);
            var parameters = new List<SimpleSqlParameter>
                {
                    new SimpleSqlParameter("@id", id)
                };
            var reader = _db.Query(sql, parameters);
            var rawEvents = CreateRawEvents(reader);
            var rawEvent = rawEvents.FirstOrDefault();
            return rawEvent != null ? CreateEvent(rawEvent) : null;
        }

        public IList<Event> Get(IList<int> ids)
        {
            const string whereClause = "WHERE e.EventID IN(@ids)";
            var sql = string.Format(EventSql, whereClause);
            var parameter = new ListSqlParameter("@ids", ids);
            var reader = _db.Query(sql, parameter);
            var rawEvents = CreateRawEvents(reader);
            return rawEvents.Select(CreateEvent).ToList();
        }

        public IList<int> FindByBunchId(int bunchId)
        {
            const string sql = "SELECT e.EventID FROM [Event] e WHERE e.BunchID = @id";
            var parameters = new List<SimpleSqlParameter>
                {
                    new SimpleSqlParameter("@id", bunchId)
                };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("EventID");
        }

        public IList<int> FindByCashgameId(int cashgameId)
        {
            const string sql = "SELECT ecg.EventID FROM [EventCashgame] ecg WHERE ecg.CashgameId = @id";
            var parameters = new List<SimpleSqlParameter>
                {
                    new SimpleSqlParameter("@id", cashgameId)
                };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("EventID");
        }

        public int Add(Event e)
        {
            const string sql = "INSERT INTO event (Name, BunchId) VALUES (@name, @bunchId) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
            var parameters = new List<SimpleSqlParameter>
                {
                    new SimpleSqlParameter("@name", e.Name),
                    new SimpleSqlParameter("@bunchId", e.BunchId)
                };
            return _db.ExecuteInsert(sql, parameters);
        }

        public void AddCashgame(int eventId, int cashgameId)
        {
            const string sql = "INSERT INTO eventcashgame (EventId, GameId) VALUES (@eventId, @cashgameId)";
            var parameters = new List<SimpleSqlParameter>
                {
                    new SimpleSqlParameter("@eventId", eventId),
                    new SimpleSqlParameter("@cashgameId", cashgameId)
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
                reader.GetIntValue("EventID"),
                reader.GetIntValue("BunchId"),
                reader.GetStringValue("Name"),
                reader.GetIntValue("LocationId"),
                reader.GetDateTimeValue("Date"));
        }
    }
}
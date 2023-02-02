using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class EventDb
{
    private readonly IDb _db;

    public EventDb(IDb db)
    {
        _db = db;
    }

    public async Task<Event> Get(string id)
    {
        var @params = new
        {
            eventId = int.Parse(id)
        };
        
        var eventDayDtos = await _db.List<EventDayDto>(EventSql.GetByIdQuery, @params);
        var events = eventDayDtos.ToEvents();
        var @event = events.FirstOrDefault();
        return @event;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var eventDayDtos = await _db.List<EventDayDto>(EventSql.GetByIdsQuery, param);
        return eventDayDtos.ToEvents();
    }

    public async Task<IList<string>> FindByBunchId(string bunchId)
    {
        var @params = new
        {
            id = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(EventSql.SearchByIdQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var @params = new
        {
            id = int.Parse(cashgameId)
        };

        return (await _db.List<int>(EventSql.SearchByCashgameQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        var @params = new
        {
            name = e.Name,
            bunchId = int.Parse(e.BunchId)
        };

        return (await _db.Insert(EventSql.AddQuery, @params)).ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Insert(EventSql.AddCashgameQuery, @params);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Execute(EventSql.RemoveCashgameQuery, @params);
    }
}
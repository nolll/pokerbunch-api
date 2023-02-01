using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
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

internal static class EventMapper
{
    internal static List<Event> ToEvents(this IEnumerable<EventDayDto> eventDayDtos)
    {
        var eventDtos = ToEventDtos(eventDayDtos);
        return eventDtos.Select(ToEvent).ToList();
    }

    private static Event ToEvent(this EventDto eventDto)
    {
        return new Event(
            eventDto.Event_Id,
            eventDto.Bunch_Id,
            eventDto.Name,
            eventDto.Location_Id,
            new Date(eventDto.StartDate),
            new Date(eventDto.EndDate));
    }

    private static IList<EventDto> ToEventDtos(IEnumerable<EventDayDto> rawEventDays)
    {
        var map = new Dictionary<string, IList<EventDayDto>>();
        foreach (var day in rawEventDays)
        {
            IList<EventDayDto> list;
            if (map.ContainsKey(day.Event_Id))
            {
                list = map[day.Event_Id];
            }
            else
            {
                list = new List<EventDayDto>();
                map[day.Event_Id] = list;
            }
            list.Add(day);
        }

        var rawEvents = new List<EventDto>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            rawEvents.Add(new EventDto(firstItem.Event_Id, firstItem.Bunch_Id, firstItem.Name, firstItem.Location_Id, firstItem.Timestamp, lastItem.Timestamp));
        }
        return rawEvents;
    }
}
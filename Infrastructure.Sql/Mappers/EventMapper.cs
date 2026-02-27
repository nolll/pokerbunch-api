using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class EventMapper
{
    internal static List<Event> ToEvents(this IEnumerable<EventDayDto> eventDayDtos)
    {
        var eventDtos = ToEventDtos(eventDayDtos);
        return eventDtos.Select(ToEvent).ToList();
    }

    private static Event ToEvent(this EventDto eventDto) => new(
        eventDto.EventId.ToString(),
        eventDto.BunchSlug,
        eventDto.Name,
        eventDto.LocationId?.ToString(),
        new Date(eventDto.StartDate),
        new Date(eventDto.EndDate));

    private static IList<EventDto> ToEventDtos(IEnumerable<EventDayDto> eventDayDtos)
    {
        var map = new Dictionary<int, IList<EventDayDto>>();
        foreach (var day in eventDayDtos)
        {
            IList<EventDayDto> list;
            if (map.TryGetValue(day.EventId, out var value))
            {
                list = value;
            }
            else
            {
                list = new List<EventDayDto>();
                map[day.EventId] = list;
            }
            list.Add(day);
        }

        var eventDtos = new List<EventDto>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            eventDtos.Add(new EventDto(firstItem.EventId, firstItem.BunchSlug, firstItem.Name, firstItem.LocationId, firstItem.Timestamp, lastItem.Timestamp));
        }
        return eventDtos;
    }
}
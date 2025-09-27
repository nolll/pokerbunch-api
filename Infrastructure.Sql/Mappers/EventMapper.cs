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
        eventDto.Event_Id.ToString(),
        eventDto.Bunch_Slug,
        eventDto.Name,
        eventDto.Location_Id?.ToString(),
        new Date(eventDto.StartDate),
        new Date(eventDto.EndDate));

    private static IList<EventDto> ToEventDtos(IEnumerable<EventDayDto> eventDayDtos)
    {
        var map = new Dictionary<int, IList<EventDayDto>>();
        foreach (var day in eventDayDtos)
        {
            IList<EventDayDto> list;
            if (map.TryGetValue(day.Event_Id, out var value))
            {
                list = value;
            }
            else
            {
                list = new List<EventDayDto>();
                map[day.Event_Id] = list;
            }
            list.Add(day);
        }

        var eventDtos = new List<EventDto>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            eventDtos.Add(new EventDto(firstItem.Event_Id, firstItem.Bunch_Slug, firstItem.Name, firstItem.Location_Id, firstItem.Timestamp, lastItem.Timestamp));
        }
        return eventDtos;
    }
}
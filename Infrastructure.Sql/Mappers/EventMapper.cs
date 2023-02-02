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

    private static Event ToEvent(this EventDto eventDto)
    {
        return new Event(
            eventDto.Event_Id.ToString(),
            eventDto.Bunch_Id.ToString(),
            eventDto.Name,
            eventDto.Location_Id.ToString(),
            new Date(eventDto.StartDate),
            new Date(eventDto.EndDate));
    }

    private static IList<EventDto> ToEventDtos(IEnumerable<EventDayDto> eventDayDtos)
    {
        var map = new Dictionary<int, IList<EventDayDto>>();
        foreach (var day in eventDayDtos)
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

        var eventDtos = new List<EventDto>();
        foreach (var key in map.Keys)
        {
            var item = map[key];
            var firstItem = item.First();
            var lastItem = item.Last();
            eventDtos.Add(new EventDto(firstItem.Event_Id, firstItem.Bunch_Id, firstItem.Name, firstItem.Location_Id, firstItem.Timestamp, lastItem.Timestamp));
        }
        return eventDtos;
    }
}
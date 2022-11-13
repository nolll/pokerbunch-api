using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Api.Models.EventModels;

public class EventListModel : List<EventModel>
{
    public EventListModel(EventList.Result eventListResult)
    {
        AddRange(eventListResult.Events.Select(o => new EventModel(o)));
    }
}
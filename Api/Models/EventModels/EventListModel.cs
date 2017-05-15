using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.EventModels
{
    [CollectionDataContract(Namespace = "", Name = "events", ItemName = "event")]
    public class EventListModel : List<EventModel>
    {
        public EventListModel(EventList.Result eventListResult)
        {
            AddRange(eventListResult.Events.Select(o => new EventModel(o)));
        }
    }
}
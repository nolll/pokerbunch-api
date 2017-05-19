using System.Runtime.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.EventModels
{
    [DataContract(Namespace = "", Name = "event")]
    public class EventModel
    {
        [DataMember(Name = "id")]
        public int Id { get; }
        [DataMember(Name = "bunchId")]
        public string BunchId { get; }
        [DataMember(Name = "name")]
        public string Name { get; }
        [DataMember(Name = "location")]
        public SmallLocationModel Location { get; }

        public EventModel(EventList.Event e)
        {
            Id = e.EventId;
            BunchId = e.BunchId;
            Name = e.Name;
            Location = e.LocationId > 0 ? new SmallLocationModel(e) : null;
        }

        public EventModel(EventDetails.Result r)
        {
            Id = r.Id;
            BunchId = r.BunchId;
            Name = r.Name;
            Location = r.LocationId > 0 ? new SmallLocationModel(r) : null;
        }
    }
}
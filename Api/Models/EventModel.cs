using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "event")]
    public class EventModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "bunchId")]
        public string BunchId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "location")]
        public SmallLocationModel Location { get; set; }

        public EventModel(EventList.Event e)
        {
            Id = e.EventId;
            BunchId = e.BunchId;
            Name = e.Name;
            Location = new SmallLocationModel(e);
        }

        public EventModel()
        {
        }
    }
}
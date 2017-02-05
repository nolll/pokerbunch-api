using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "event")]
    public class EventModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public EventModel(EventList.Event e)
            : this(e.EventId, e.Name)
        {
        }

        //public EventModel(GetLocation.Result location)
        //    : this(location.Id, location.Name, location.Slug)
        //{
        //}

        //public EventModel(AddLocation.Result location)
        //    : this(location.Id, location.Name, location.Slug)
        //{
        //}

        private EventModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public EventModel()
        {
        }
    }
}
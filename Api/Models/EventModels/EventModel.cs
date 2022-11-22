using System.Text.Json.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.EventModels;

public class EventModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("bunchId")]
    public string BunchId { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("startDate")]
    public string StartDate { get; }
    
    [JsonPropertyName("location")]
    public SmallLocationModel Location { get; }

    public EventModel(EventList.Event e)
    {
        Id = e.EventId;
        BunchId = e.BunchId;
        Name = e.Name;
        StartDate = e.StartDate?.IsoString;
        Location = e.LocationId != null ? new SmallLocationModel(e) : null;
    }

    public EventModel(EventDetails.Result r)
    {
        Id = r.Id;
        BunchId = r.BunchId;
        Name = r.Name;
        StartDate = r.StartDate?.IsoString;
        Location = r.LocationId != null ? new SmallLocationModel(r) : null;
    }

    [JsonConstructor]
    public EventModel(string id, string bunchId, string name, string startDate, SmallLocationModel location)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        StartDate = startDate;
        Location = location;
    }
}
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

[DataContract(Namespace = "", Name = "location")]
public class SmallLocationModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "name")]
    public string Name { get; }

    public SmallLocationModel(CashgameDetails.Result details)
        : this(details.LocationId, details.LocationName)
    {
    }

    public SmallLocationModel(CashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventCashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(PlayerCashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventList.Event item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventDetails.Result e)
        : this(e.LocationId, e.LocationName)
    {
    }

    public SmallLocationModel(int id, string name)
        : this(id.ToString(), name)
    {
    }

    public SmallLocationModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
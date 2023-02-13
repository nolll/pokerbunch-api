namespace Core.Entities;

public class Event : IEntity
{
    public string Id { get; }
    public string BunchId { get; }
    public string Name { get; }
    public string? LocationId { get; }
    public Date StartDate { get; }
    public Date EndDate { get; }

    public Event(string id, string bunchId, string name)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        StartDate = Date.Null();
        EndDate = Date.Null();
    }

    public Event(string id, string bunchId, string name, string? locationId, Date startDate, Date endDate)
        : this(id, bunchId, name)
    {
        LocationId = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool HasGames => HasLocation && HasStartDate && HasEndDate;
    private bool HasLocation => LocationId != null;
    private bool HasStartDate => !StartDate.IsNull;
    private bool HasEndDate => !EndDate.IsNull;
}
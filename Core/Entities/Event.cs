namespace Core.Entities;

public class Event(string id, string bunchSlug, string name) : IEntity
{
    public string Id { get; } = id;
    public string BunchSlug { get; } = bunchSlug;
    public string Name { get; } = name;
    public string? LocationId { get; }
    public Date StartDate { get; } = Date.Null();
    public Date EndDate { get; } = Date.Null();

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
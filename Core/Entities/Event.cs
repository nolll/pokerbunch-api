namespace Core.Entities;

public class Event : IEntity
{
    public int Id { get; }
    public int BunchId { get; private set; }
    public string Name { get; private set; }
    public int LocationId { get; }
    public Date StartDate { get; }
    public Date EndDate { get; }

    public Event(int id, int bunchId, string name)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
    }

    public Event(int id, int bunchId, string name, int locationId, Date startDate, Date endDate)
        : this(id, bunchId, name)
    {
        LocationId = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool HasGames => HasLocation && HasStartDate && HasEndDate;
    private bool HasLocation => LocationId != 0;
    private bool HasStartDate => !StartDate.IsNull;
    private bool HasEndDate => !EndDate.IsNull;
}
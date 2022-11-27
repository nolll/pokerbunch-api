namespace Infrastructure.Sql.Classes;

public class RawEvent
{
    public string Id { get; }
    public string BunchId { get; }
    public string Name { get; }
    public string LocationId { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public RawEvent(string id, string bunchId, string name, string locationId, DateTime startDate, DateTime endDate)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        LocationId = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
using System;

namespace Infrastructure.Sql.Classes;

public class RawEvent
{
    public int Id { get; }
    public int BunchId { get; }
    public string Name { get; }
    public int LocationId { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public RawEvent(int id, int bunchId, string name, int locationId, DateTime startDate, DateTime endDate)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        LocationId = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
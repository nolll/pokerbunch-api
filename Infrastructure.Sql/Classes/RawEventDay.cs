namespace Infrastructure.Sql.Classes;

public class RawEventDay
{
    public string Id { get; }
    public string BunchId { get; }
    public string Name { get; }
    public string LocationId { get; }
    public DateTime Date { get; }

    public RawEventDay(string id, string bunchId, string name, string locationId, DateTime date)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        LocationId = locationId;
        Date = date;
    }
}
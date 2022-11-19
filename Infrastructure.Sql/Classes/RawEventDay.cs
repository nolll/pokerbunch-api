namespace Infrastructure.Sql.Classes;

public class RawEventDay
{
    public int Id { get; }
    public int BunchId { get; }
    public string Name { get; }
    public int LocationId { get; }
    public DateTime Date { get; }

    public RawEventDay(int id, int bunchId, string name, int locationId, DateTime date)
    {
        Id = id;
        BunchId = bunchId;
        Name = name;
        LocationId = locationId;
        Date = date;
    }
}
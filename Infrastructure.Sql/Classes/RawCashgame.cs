namespace Infrastructure.Sql.Classes;

public class RawCashgame
{
    public int Id { get; }
    public int BunchId { get; }
    public int LocationId { get; }
    public int EventId { get; }
    public int Status { get; }
    public DateTime Date { get; }

    public RawCashgame(int id, int bunchId, int locationId, int eventId, int status, DateTime date)
    {
        Id = id;
        BunchId = bunchId;
        LocationId = locationId;
        EventId = eventId;
        Status = status;
        Date = date;
    }
}
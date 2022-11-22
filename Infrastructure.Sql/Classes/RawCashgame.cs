namespace Infrastructure.Sql.Classes;

public class RawCashgame
{
    public string Id { get; }
    public string BunchId { get; }
    public string LocationId { get; }
    public string EventId { get; }
    public int Status { get; }
    public DateTime Date { get; }

    public RawCashgame(string id, string bunchId, string locationId, string eventId, int status, DateTime date)
    {
        Id = id;
        BunchId = bunchId;
        LocationId = locationId;
        EventId = eventId;
        Status = status;
        Date = date;
    }
}
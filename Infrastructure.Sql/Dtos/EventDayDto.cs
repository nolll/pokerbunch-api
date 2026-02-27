namespace Infrastructure.Sql.Dtos;

public class EventDayDto
{
    public int EventId { get; set; }
    public string BunchSlug { get; set; } = "";
    public string Name { get; set; } = "";
    public int? LocationId { get; set; }
    public DateTime Timestamp { get; set; }
}
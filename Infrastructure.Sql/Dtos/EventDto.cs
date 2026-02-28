namespace Infrastructure.Sql.Dtos;

public class EventDto(int id, string bunchSlug, string name, int? locationId, DateTime startDate, DateTime endDate)
{
    public int EventId { get; } = id;
    public string BunchSlug { get; } = bunchSlug;
    public string Name { get; } = name;
    public int? LocationId { get; } = locationId;
    public DateTime StartDate { get; } = startDate;
    public DateTime EndDate { get; } = endDate;
}
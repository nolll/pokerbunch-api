// ReSharper disable InconsistentNaming
namespace Infrastructure.Sql.Dtos;

public class EventDto(int id, int bunchId, string bunchSlug, string name, int? locationId, DateTime startDate, DateTime endDate)
{
    public int Event_Id { get; } = id;
    public int Bunch_Id { get; } = bunchId;
    public string Bunch_Slug { get; } = bunchSlug;
    public string Name { get; } = name;
    public int? Location_Id { get; } = locationId;
    public DateTime StartDate { get; } = startDate;
    public DateTime EndDate { get; } = endDate;
}
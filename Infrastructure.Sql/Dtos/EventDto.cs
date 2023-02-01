// ReSharper disable InconsistentNaming
namespace Infrastructure.Sql.Dtos;

public class EventDto
{
    public string Event_Id { get; }
    public string Bunch_Id { get; }
    public string Name { get; }
    public string Location_Id { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    
    public EventDto(string id, string bunchId, string name, string locationId, DateTime startDate, DateTime endDate)
    {
        Event_Id = id;
        Bunch_Id = bunchId;
        Name = name;
        Location_Id = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
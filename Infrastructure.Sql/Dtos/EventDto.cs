// ReSharper disable InconsistentNaming
namespace Infrastructure.Sql.Dtos;

public class EventDto
{
    public int Event_Id { get; }
    public int Bunch_Id { get; }
    public string Name { get; }
    public int? Location_Id { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    
    public EventDto(int id, int bunchId, string name, int? locationId, DateTime startDate, DateTime endDate)
    {
        Event_Id = id;
        Bunch_Id = bunchId;
        Name = name;
        Location_Id = locationId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
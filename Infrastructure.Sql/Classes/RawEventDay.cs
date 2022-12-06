namespace Infrastructure.Sql.Classes;

public class RawEventDay
{
    public string Event_Id { get; set; }
    public string Bunch_Id { get; set; }
    public string Name { get; set; }
    public string Location_Id { get; set; }
    public DateTime Date { get; set; }

    public RawEventDay()
    {
    }

    public RawEventDay(string eventId, string bunchId, string name, string locationId, DateTime date)
    {
        Event_Id = eventId;
        Bunch_Id = bunchId;
        Name = name;
        Location_Id = locationId;
        Date = date;
    }
}
using System;

namespace Infrastructure.Sql.Classes
{
	public class RawCashgame
    {
	    public int Id { get; private set; }
        public int BunchId { get; private set; }
	    public int LocationId { get; private set; }
	    public int EventId { get; private set; }
	    public int Status { get; private set; }
	    public DateTime Date { get; private set; }

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
}
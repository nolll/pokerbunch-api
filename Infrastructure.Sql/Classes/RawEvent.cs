using System;

namespace Infrastructure.Sql.Classes
{
    public class RawEvent
    {
        public int Id { get; private set; }
        public int BunchId { get; private set; }
        public string Name { get; private set; }
        public int LocationId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public RawEvent(int id, int bunchId, string name, int locationId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            BunchId = bunchId;
            Name = name;
            LocationId = locationId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
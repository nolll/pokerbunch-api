using System;

namespace Infrastructure.Storage.Classes
{
    public class RawEventDay
    {
        public int Id { get; private set; }
        public int BunchId { get; private set; }
        public string Name { get; private set; }
        public int LocationId { get; private set; }
        public DateTime Date { get; private set; }

        public RawEventDay(int id, int bunchId, string name, int locationId, DateTime date)
        {
            Id = id;
            BunchId = bunchId;
            Name = name;
            LocationId = locationId;
            Date = date;
        }
    }
}
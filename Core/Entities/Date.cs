using System;

namespace Core.Entities
{
    public class Date : IComparable<Date>
    {
        public int Month { get; }
        public int Day { get; }
        public int Year { get; }

        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public Date(DateTime dateTime)
            : this(dateTime.Year, dateTime.Month, dateTime.Day)
        {
        }

        public string IsoString
        {
            get { return string.Format("{0}-{1}-{2}", Year.ToString("D4"), Month.ToString("D2"), Day.ToString("D2")); }
        }

        public DateTime UtcMidninght
        {
            get { return new DateTime(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc); }
        }

        public static Date Parse(string s)
        {
            var d = DateTime.Parse(s);
            return new Date(d.Year, d.Month, d.Day);
        }

        //todo: override gethashcode
        public override bool Equals(object obj)
        {
            var other = obj as Date;
            return other != null && Year == other.Year && Month == other.Month && Day == other.Day;
        }

        public int CompareTo(Date that)
        {
            return UtcMidninght.CompareTo(that.UtcMidninght);
        }

        public bool IsNull
        {
            get
            {
                var minDate = new Date(DateTime.MinValue);
                return CompareTo(minDate) == 0;
            }
        }
    }
}
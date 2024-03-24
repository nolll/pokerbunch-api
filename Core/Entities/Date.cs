using System;

namespace Core.Entities;

public class Date(int year, int month, int day) : IComparable<Date>
{
    public int Month { get; } = month;
    public int Day { get; } = day;
    public int Year { get; } = year;

    private string YearString => Year.ToString("D4");
    private string MonthString => Month.ToString("D2");
    private string DayString => Day.ToString("D2");
    public string IsoString => $"{YearString}-{MonthString}-{DayString}";
    private DateTime UtcMidninght => new(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc);

    public Date(DateTime dateTime)
        : this(dateTime.Year, dateTime.Month, dateTime.Day)
    {
    }

    public static Date Parse(string s)
    {
        var d = DateTime.Parse(s);
        return new Date(d.Year, d.Month, d.Day);
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Date;
        return other != null && Year == other.Year && Month == other.Month && Day == other.Day;
    }

    protected bool Equals(Date other) => Month == other.Month && Day == other.Day && Year == other.Year;
    public override int GetHashCode() => HashCode.Combine(Month, Day, Year);
    public int CompareTo(Date? that) => UtcMidninght.CompareTo(that?.UtcMidninght);

    public bool IsNull
    {
        get
        {
            var minDate = new Date(DateTime.MinValue);
            return CompareTo(minDate) == 0;
        }
    }

    public static Date Null() => new(DateTime.MinValue);
}
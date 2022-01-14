using System;

namespace Core.Entities;

public class Time : IComparable<Time>
{
    private TimeSpan _timeSpan;

    public static Time FromMinutes(int minutes)
    {
        return new Time(TimeSpan.FromMinutes(minutes));
    }

    private Time(TimeSpan timeSpan)
    {
        _timeSpan = timeSpan;
    }

    public int Minutes => Convert.ToInt32(Math.Round(_timeSpan.TotalMinutes));

    public int CompareTo(Time other)
    {
        return _timeSpan.CompareTo(other._timeSpan);
    }

    public override string ToString()
    {
        var minutes = Minutes;
        var h = (int) Math.Floor((double) minutes/60);
        var m = minutes%60;
        if (h > 0 && m > 0)
            return h + "h " + m + "m";
        if (h > 0)
            return h + "h";
        return m + "m";
    }
}
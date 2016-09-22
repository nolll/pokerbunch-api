using System;

namespace Tests.Common.Builders
{
    public class DateTimeBuilder
    {
        private int _year;
        private int _month;
        private int _day;
        private int _hour;
        private int _minute;
        private int _second;
        private TimeZoneInfo _timeZone;

        public DateTimeBuilder()
        {
            _year = 2001;
            _month = 1;
            _day = 1;
            _hour = 1;
            _minute = 1;
            _second = 1;
            _timeZone = TimeZoneInfo.Utc;
        }

        public DateTime Build()
        {
            var dateTime = new DateTime(_year, _minute, _day, _hour, _minute, _second);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, _timeZone);
        }

        public DateTimeBuilder WithYear(int year)
        {
            _year = year;
            return this;
        }

        public DateTimeBuilder WithMonth(int month)
        {
            _month = month;
            return this;
        }

        public DateTimeBuilder WithDay(int day)
        {
            _day = day;
            return this;
        }

        public DateTimeBuilder WithHour(int hour)
        {
            _hour = hour;
            return this;
        }

        public DateTimeBuilder WithMinute(int minute)
        {
            _minute = minute;
            return this;
        }

        public DateTimeBuilder WithSeconds(int second)
        {
            _second = second;
            return this;
        }

        public DateTimeBuilder AsLocal()
        {
            _timeZone = TestData.TimeZoneLocal;
            return this;
        }

        public DateTimeBuilder AsUtc()
        {
            _timeZone = TimeZoneInfo.Utc;
            return this;
        }
    }
}
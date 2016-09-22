using System.Collections.Generic;
using Core.Services;

namespace Core.UseCases
{
    public class AddBunchForm
    {
        public Result Execute()
        {
            var timeZones = TimeZoneService.GetTimeZones();
            var currencyLayouts = Globalization.GetCurrencyLayouts();

            return new Result(timeZones, currencyLayouts);
        }

        public class Result
        {
            public IList<TimeZoneItem> TimeZones { get; private set; }
            public IList<string> CurrencyLayouts { get; private set; }

            public Result(IList<TimeZoneItem> timeZones, IList<string> currencyLayouts)
            {
                TimeZones = timeZones;
                CurrencyLayouts = currencyLayouts;
            }
        }

        public class TimeZoneItem
        {
            public string Id { get; private set; }
            public string Name { get; private set; }

            public TimeZoneItem(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }
}
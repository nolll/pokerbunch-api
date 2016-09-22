using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Core.Services
{
    public static class TimeZoneService
    {
        public static List<AddBunchForm.TimeZoneItem> GetTimeZones()
        {
            var timeZones = Globalization.GetTimezones();
            return timeZones.Select(t => new AddBunchForm.TimeZoneItem(t.Id, t.DisplayName)).ToList();
        }
    }
}
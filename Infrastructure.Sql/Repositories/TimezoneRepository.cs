using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Sql.Repositories
{
    public class TimezoneRepository : ITimezoneRepository
    {
        public IList<Timezone> List()
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            return timezones.Select(o => new Timezone(o.Id, o.DisplayName)).ToList();
        }
    }
}
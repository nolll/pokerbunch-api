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
            return TimeZoneInfo.GetSystemTimeZones().Select(o => new Timezone(o.Id, o.DisplayName)).ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels
{
    [CollectionDataContract(Namespace = "", Name = "timezones", ItemName = "timezone")]
    public class TimezoneListModel : List<TimezoneModel>
    {
        public TimezoneListModel(GetTimezoneList.Result timezoneListResult)
        {
            AddRange(timezoneListResult.Timezones.Select(o => new TimezoneModel(o)));
        }
    }
}
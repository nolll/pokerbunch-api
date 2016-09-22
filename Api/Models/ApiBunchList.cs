using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "bunches", ItemName = "bunch")]
    public class ApiBunchList : List<ApiBunch>
    {
        public ApiBunchList(BunchList.Result bunchListResult)
        {
            AddRange(bunchListResult.Bunches.Select(o => new ApiBunch(o.Id, o.Slug, o.DisplayName)));
        }

        public ApiBunchList()
        {
        }
    }
}
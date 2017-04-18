using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "years", ItemName = "year")]
    public class CashgameYearListModel : List<CashgameYearListItemModel>
    {
        public CashgameYearListModel(CashgameYearList.Result listResult)
        {
            AddRange(listResult.Years.Select(o => new CashgameYearListItemModel(listResult.Slug, o)).ToList());
        }

        public CashgameYearListModel()
        {
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "cashgames", ItemName = "cashgame")]
    public class CashgameListModel : List<CashgameListItemModel>
    {
        public CashgameListModel(CashgameList.Result listResult)
        {
            AddRange(listResult.Items.Select(o => new CashgameListItemModel(o)));
        }

        public CashgameListModel()
        {
        }
    }
}
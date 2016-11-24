using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "toplist", ItemName = "player")]
    public class ApiCashgameTopList : List<ApiCashgameTopListItem>
    {
        public ApiCashgameTopList(TopList.Result topListResult)
        {
            AddRange(topListResult.Items.Select(o => new ApiCashgameTopListItem(o.Name, o.Winnings.Amount)));
        }

        public ApiCashgameTopList()
        {
        }
    }

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
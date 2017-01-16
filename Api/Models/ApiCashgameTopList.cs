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
}
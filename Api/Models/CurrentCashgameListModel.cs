using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "cashgames", ItemName = "cashgame")]
    public class CurrentCashgameListModel : List<ApiCurrentGame>
    {
        public CurrentCashgameListModel(CurrentCashgames.Result listResult)
        {
            AddRange(listResult.Games.Select(o => new ApiCurrentGame(o)).ToList());
        }

        public CurrentCashgameListModel()
        {
        }
    }
}
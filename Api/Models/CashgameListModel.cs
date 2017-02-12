using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "cashgamelist")]
    public class CashgameListModel
    {
        [DataMember(Name = "bunch")]
        public CashgameBunchModel Bunch { get; set; }
        [DataMember(Name = "cashgames")]
        public IList<CashgameListItemModel> Cashgames { get; set; } 

        public CashgameListModel(CashgameList.Result listResult)
        {
            Bunch = new CashgameBunchModel(listResult);
            Cashgames = listResult.Items.Select(o => new CashgameListItemModel(o)).ToList();
        }

        public CashgameListModel()
        {
        }
    }
}
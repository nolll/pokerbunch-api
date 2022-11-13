using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameListModel : List<CashgameListItemModel>
{
    public CashgameListModel(CashgameList.Result listResult)
    {
        AddRange(listResult.Items.Select(o => new CashgameListItemModel(o)).ToList());
    }

    public CashgameListModel(EventCashgameList.Result listResult)
    {
        AddRange(listResult.Items.Select(o => new CashgameListItemModel(o)).ToList());
    }

    public CashgameListModel(PlayerCashgameList.Result listResult)
    {
        AddRange(listResult.Items.Select(o => new CashgameListItemModel(o)).ToList());
    }
}
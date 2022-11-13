using System.Collections.Generic;
using System.Linq;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CurrentCashgameListModel : List<ApiCurrentGame>
{
    public CurrentCashgameListModel(CurrentCashgames.Result listResult, UrlProvider urls)
    {
        AddRange(listResult.Games.Select(o => new ApiCurrentGame(o, urls)).ToList());
    }
}
using System.Collections.Generic;
using System.Linq;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameYearListModel : List<CashgameYearListItemModel>
{
    public CashgameYearListModel(CashgameYearList.Result listResult, UrlProvider urls)
    {
        AddRange(listResult.Years.Select(o => new CashgameYearListItemModel(listResult.Slug, o, urls)).ToList());
    }
}
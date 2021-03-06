using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[CollectionDataContract(Namespace = "", Name = "years", ItemName = "year")]
public class CashgameYearListModel : List<CashgameYearListItemModel>
{
    public CashgameYearListModel(CashgameYearList.Result listResult, UrlProvider urls)
    {
        AddRange(listResult.Years.Select(o => new CashgameYearListItemModel(listResult.Slug, o, urls)).ToList());
    }
}
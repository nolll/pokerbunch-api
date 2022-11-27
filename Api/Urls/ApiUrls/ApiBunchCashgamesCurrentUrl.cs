﻿using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchCashgamesCurrentUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiBunchCashgamesCurrentUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListCurrentByBunch, RouteReplace.BunchId(_bunchId));
}
﻿using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiAdminClearCacheUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Admin.ClearCache;
}
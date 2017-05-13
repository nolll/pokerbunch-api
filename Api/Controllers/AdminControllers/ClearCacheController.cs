using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.AdminControllers
{
    public class ClearCacheController : BaseController
    {
        [Route(ApiRoutes.Admin.ClearCache)]
        [HttpPost]
        [ApiAuthorize]
        public CacheClearedModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new CacheClearedModel(result.ClearCount);
        }
    }
}
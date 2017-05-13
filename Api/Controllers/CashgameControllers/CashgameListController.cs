using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameListController : BaseController
    {
        [Route(ApiRoutes.CashgameList)]
        [Route(ApiRoutes.CashgameListWithYear)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string slug, int? year = null)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }
    }
}
using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameYearsController : BaseController
    {
        [Route(ApiRoutes.CashgameYears)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameYearListModel Years(string id)
        {
            var listResult = UseCase.CashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, id));
            return new CashgameYearListModel(listResult);
        }
    }
}
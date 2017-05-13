using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameDeleteController : BaseController
    {
        [Route(ApiRoutes.CashgameDelete)]
        [HttpDelete]
        [ApiAuthorize]
        public CashgameDeletedModel Delete(int id)
        {
            var deleteRequest = new DeleteCashgame.Request(CurrentUserName, id);
            UseCase.DeleteCashgame.Execute(deleteRequest);
            return new CashgameDeletedModel(id);
        }
    }
}
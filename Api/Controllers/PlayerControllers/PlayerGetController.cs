using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.PlayerControllers
{
    public class PlayerGetController : BaseController
    {
        [Route(ApiRoutes.PlayerGet)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerModel Get(int id)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, id));
            return new PlayerModel(getPlayerResult);
        }
    }
}
using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class PlayerController : BaseApiController
    {
        [Route(ApiRoutes.PlayerList)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string slug)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, slug));
            return new PlayerListModel(playerListResult);
        }

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
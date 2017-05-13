using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.PlayerControllers
{
    public class PlayerListController : BaseController
    {
        [Route(ApiRoutes.PlayerList)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string slug)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, slug));
            return new PlayerListModel(playerListResult);
        }
    }
}
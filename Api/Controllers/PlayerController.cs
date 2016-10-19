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
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult GetList(string slug)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, slug));
            var model = new PlayerListModel(playerListResult);
            return Ok(model);
        }

        [Route(ApiRoutes.PlayerGet)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, id));
            var bunchModel = new PlayerModel(getPlayerResult);
            return Ok(bunchModel);
        }
    }
}
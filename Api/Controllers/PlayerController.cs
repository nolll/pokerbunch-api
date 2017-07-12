using System.Web.Http;
using Api.Auth;
using Api.Models.PlayerModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class PlayerController : BaseController
    {
        [Route(ApiRoutes.PlayerGet)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerModel Get(int id)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, id));
            return new PlayerModel(getPlayerResult);
        }

        [Route(ApiRoutes.BunchPlayerList)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string slug)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, slug));
            return new PlayerListModel(playerListResult);
        }

        [Route(ApiRoutes.BunchPlayerAdd)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerModel Add(string slug, [FromBody] PlayerAddPostModel post)
        {
            var result = UseCase.AddPlayer.Execute(new AddPlayer.Request(CurrentUserName, slug, post.Name));
            return Get(result.Id);
        }

        [Route(ApiRoutes.PlayerDelete)]
        [HttpDelete]
        [ApiAuthorize]
        public PlayerDeleteModel Delete(int id)
        {
            var deleteRequest = new DeletePlayer.Request(CurrentUserName, id);
            UseCase.DeletePlayer.Execute(deleteRequest);
            return new PlayerDeleteModel(id);
        }

    }
}
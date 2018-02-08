using System.Web.Http;
using Api.Auth;
using Api.Extensions;
using Api.Models.PlayerModels;
using Core.UseCases;
using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Controllers
{
    public class PlayerController : BaseController
    {
        [Route(ApiRoutes.Player)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerModel Get(int playerId)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, playerId));
            return new PlayerModel(getPlayerResult);
        }

        [Route(ApiRoutes.PlayersByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string bunchId)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, bunchId));
            return new PlayerListModel(playerListResult);
        }

        [Route(ApiRoutes.PlayersByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerModel Add(string bunchId, [FromBody] PlayerAddPostModel post)
        {
            var result = UseCase.AddPlayer.Execute(new AddPlayer.Request(CurrentUserName, bunchId, post.Name));
            return Get(result.Id);
        }

        [Route(ApiRoutes.Player)]
        [HttpDelete]
        [ApiAuthorize]
        public PlayerDeleteModel Delete(int playerId)
        {
            var deleteRequest = new DeletePlayer.Request(CurrentUserName, playerId);
            UseCase.DeletePlayer.Execute(deleteRequest);
            return new PlayerDeleteModel(playerId);
        }

        [Route(ApiRoutes.PlayerInvite)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerInvitedModel Invite(int playerId, [FromBody] PlayerInvitePostModel post)
        {
            var registerUrl = new AddUserUrl().Absolute();
            var joinBunchUrlFormat = new JoinBunchUrl("{0}").Absolute();
            var joinBunchWithCodeUrlFormat = new JoinBunchUrl("{0}", "{1}").Absolute();
            var deleteRequest = new InvitePlayer.Request(CurrentUserName, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
            UseCase.InvitePlayer.Execute(deleteRequest);
            return new PlayerInvitedModel(playerId);
        }
    }
}
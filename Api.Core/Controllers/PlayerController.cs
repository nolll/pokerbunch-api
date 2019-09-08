using Api.Auth;
using Api.Extensions;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class PlayerController : BaseController
    {
        private readonly UrlProvider _urls;

        public PlayerController(Settings settings, UrlProvider urls) : base(settings)
        {
            _urls = urls;
        }

        [Route(ApiRoutes.Player.Get)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerModel Get(int playerId)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, playerId));
            return new PlayerModel(getPlayerResult);
        }

        [Route(ApiRoutes.Player.ListByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string bunchId)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, bunchId));
            return new PlayerListModel(playerListResult);
        }

        [Route(ApiRoutes.Player.ListByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerModel Add(string bunchId, [FromBody] PlayerAddPostModel post)
        {
            var result = UseCase.AddPlayer.Execute(new AddPlayer.Request(CurrentUserName, bunchId, post.Name));
            return Get(result.Id);
        }

        [Route(ApiRoutes.Player.Get)]
        [HttpDelete]
        [ApiAuthorize]
        public PlayerDeleteModel Delete(int playerId)
        {
            var deleteRequest = new DeletePlayer.Request(CurrentUserName, playerId);
            UseCase.DeletePlayer.Execute(deleteRequest);
            return new PlayerDeleteModel(playerId);
        }

        [Route(ApiRoutes.Player.Invite)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerInvitedModel Invite(int playerId, [FromBody] PlayerInvitePostModel post)
        {
            var registerUrl = _urls.Site.AddUser.Absolute();
            var joinBunchUrlFormat = _urls.Site.JoinBunch("{0}").Absolute();
            var joinBunchWithCodeUrlFormat = _urls.Site.JoinBunch("{0}", "{1}").Absolute();
            var deleteRequest = new InvitePlayer.Request(CurrentUserName, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
            UseCase.InvitePlayer.Execute(deleteRequest);
            return new PlayerInvitedModel(playerId);
        }
    }
}
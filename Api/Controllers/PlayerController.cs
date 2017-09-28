using System.Web.Http;
using Api.Auth;
using Api.Extensions;
using Api.Models.PlayerModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Controllers
{
    public class PlayerController : BaseController
    {
        [Route(ApiPlayerUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerModel Get(int id)
        {
            var getPlayerResult = UseCase.GetPlayer.Execute(new GetPlayer.Request(CurrentUserName, id));
            return new PlayerModel(getPlayerResult);
        }

        [Route(ApiBunchPlayersUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public PlayerListModel GetList(string bunchId)
        {
            var playerListResult = UseCase.GetPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, bunchId));
            return new PlayerListModel(playerListResult);
        }

        [Route(ApiBunchPlayersUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerModel Add(string bunchId, [FromBody] PlayerAddPostModel post)
        {
            var result = UseCase.AddPlayer.Execute(new AddPlayer.Request(CurrentUserName, bunchId, post.Name));
            return Get(result.Id);
        }

        [Route(ApiPlayerUrl.Route)]
        [HttpDelete]
        [ApiAuthorize]
        public PlayerDeleteModel Delete(int id)
        {
            var deleteRequest = new DeletePlayer.Request(CurrentUserName, id);
            UseCase.DeletePlayer.Execute(deleteRequest);
            return new PlayerDeleteModel(id);
        }

        [Route(ApiPlayerInviteUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerInvitedModel Invite(int id, [FromBody] PlayerInvitePostModel post)
        {
            var registerUrl = new AddUserUrl().Absolute();
            var joinBunchUrlFormat = new JoinBunchUrl("{0}").Absolute();
            var joinBunchWithCodeUrlFormat = new JoinBunchUrl("{0}", "{1}").Absolute();
            var deleteRequest = new InvitePlayer.Request(CurrentUserName, id, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
            UseCase.InvitePlayer.Execute(deleteRequest);
            return new PlayerInvitedModel(id);
        }
    }
}
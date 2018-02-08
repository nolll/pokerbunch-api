using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class BunchJoinController : BaseController
    {
        [Route(ApiRoutes.BunchJoin)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerJoinedModel Join(string bunchId, [FromBody] JoinBunchPostModel post)
        {
            var request = new JoinBunch.Request(CurrentUserName, bunchId, post.Code);
            var result = UseCase.JoinBunch.Execute(request);
            return new PlayerJoinedModel(result.PlayerId);
        }
    }
}
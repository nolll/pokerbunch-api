using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    [Route(ApiBunchJoinUrl.Route)]
    [ApiAuthorize]
    public class BunchJoinController : BaseController
    {
        [HttpPost]
        public PlayerJoinedModel Join(string slug, [FromBody] JoinBunchPostModel post)
        {
            var request = new JoinBunch.Request(CurrentUserName, slug, post.Code);
            var result = UseCase.JoinBunch.Execute(request);
            return new PlayerJoinedModel(result.PlayerId);
        }
    }
}
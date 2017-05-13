using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.UserControllers
{
    public class UserListController : BaseController
    {
        [Route(ApiRoutes.UserList)]
        [HttpGet]
        [ApiAuthorize]
        public UserListModel List()
        {
            var userListResult = UseCase.UserList.Execute(new UserList.Request(CurrentUserName));
            return new UserListModel(userListResult);
        }
    }
}
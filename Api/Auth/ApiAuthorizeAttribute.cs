using Microsoft.AspNetCore.Authorization;

namespace Api.Auth
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public ApiAuthorizeAttribute() : base("UserPolicy")
        {
        }
    }

    public class ApiNoAuthorizeAttribute : AuthorizeAttribute
    {
        public ApiNoAuthorizeAttribute() : base("NoUserPolicy")
        {
        }
    }
}
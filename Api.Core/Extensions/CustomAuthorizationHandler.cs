using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Api.Extensions
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthRequirement requirement)
        {
            // Your logic here... or anything else you need to do.
            //if (context.User.IsInRole("fooBar"))
            //{
                // Call 'Succeed' to mark current requirement as passed
                context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }
}
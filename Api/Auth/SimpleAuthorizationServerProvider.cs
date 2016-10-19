using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Cache;
using Core.Exceptions;
using Core.UseCases;
using Microsoft.Owin.Security.OAuth;

namespace Api.Auth
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            string clientId;
            string clientSecret;
            if (context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                var request = new VerifyAppKey.Request(clientId);
                var result = UseCase.VerifyAppKey.Execute(request);

                if (result.IsValid)
                {
                    context.Validated(clientId);
                    return;
                }
            }
            context.Rejected();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var loginRequest = new Login.Request(context.UserName, context.Password);
                var loginResult = UseCase.Login.Execute(loginRequest);

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, loginResult.UserName));

                context.Validated(identity);
            }
            catch (LoginException e)
            {
                context.SetError("invalid_grant", e.Message);
            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", e.Message);
            }
        }

        private UseCaseContainer UseCase => new Bootstrapper(ApiSettings.ConnectionString).UseCases;
    }
}
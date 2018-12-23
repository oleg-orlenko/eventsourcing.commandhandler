using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Authentication
{
    public class CustomAuthenticationHandler : IAuthenticationHandler
    {
        public const string Scheme = "custom";

        private HttpContext context;

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            AuthenticateResult result;
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var listOfClaims = new List<Claim>();
                listOfClaims.Add(new Claim(ClaimTypes.Name, value));
                var claimsIdentity = new ClaimsIdentity(listOfClaims, "customAuthenticationType");
                var principal = new ClaimsPrincipal(claimsIdentity);

                var ticket = new AuthenticationTicket(principal, Scheme);
                result = AuthenticateResult.Success(ticket);
            }
            else
            {
                result = AuthenticateResult.Fail("No Authorization header was found");
            }

            return Task.FromResult(result);
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.context = context;

            return Task.CompletedTask;
        }
    }
}

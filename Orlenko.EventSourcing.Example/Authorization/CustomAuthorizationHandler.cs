using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Authorization
{
    public class CustomAuthorizationHandler : AuthorizationHandler<MyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            
            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoginAuthenticationWithCustom
{
    internal class ItalianRequirement: AuthorizationHandler<ItalianRequirement>, IAuthorizationRequirement
    {
        public ItalianRequirement() { }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ItalianRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);

            }

            if (context.User.HasClaim(claim => 
                claim.Type == ClaimTypes.Country && 
                claim.Value == "Italia"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}

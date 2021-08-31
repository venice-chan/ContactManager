using System.Threading.Tasks;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ContactManager.Authorization
{
  public class ContactAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
    {
      if (context.User == null)
      {
        return Task.CompletedTask;
      }
      if (context.User.IsInRole(ContactRole.Administrator))
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
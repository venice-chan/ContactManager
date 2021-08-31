using System.Threading.Tasks;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ContactManager.Authorization
{
  public class ContactManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
    {
      if (context.User == null)
      {
        return Task.CompletedTask;
      }
      if (requirement.Name != ContactOperationName.Approve && requirement.Name != ContactOperationName.Reject)
      {
        return Task.CompletedTask;
      }
      if (context.User.IsInRole(ContactRole.Manager))
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
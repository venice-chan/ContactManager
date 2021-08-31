using System.Threading.Tasks;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.Authorization
{
  public class ContactOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
  {
    protected UserManager<IdentityUser> _userManager;
    public ContactOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
    {
      _userManager = userManager;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
    {
      if (context.User == null || resource == null)
      {
        return Task.CompletedTask;
      }

      if (requirement.Name != ContactOperationName.Create &&
        requirement.Name != ContactOperationName.Read &&
        requirement.Name != ContactOperationName.Update &&
        requirement.Name != ContactOperationName.Delete
      )
      {
        return Task.CompletedTask;
      }

      if (resource.OwnerId == _userManager.GetUserId(context.User))
      {
        context.Succeed(requirement);
      }
      return Task.CompletedTask;
    }
  }
}
using ContactManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactManager.Pages.Contacts
{
  public class BasePageModel : PageModel
  {
    protected ApplicationDbContext Context { get; }
    protected IAuthorizationService AuthorizationService { get; }
    protected UserManager<IdentityUser> UserManager { get; }


    public BasePageModel(ApplicationDbContext applicationDbContext, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
    {
      Context = applicationDbContext;
      AuthorizationService = authorizationService;
      UserManager = userManager;
    }

  }
}
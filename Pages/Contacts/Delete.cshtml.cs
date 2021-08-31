using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ContactManager.Authorization;

namespace ContactManager.Pages.Contacts
{
  public class DeleteModel : BasePageModel
  {
    public DeleteModel(
    ApplicationDbContext context,
    IAuthorizationService authorizationService,
    UserManager<IdentityUser> userManager)
    : base(context, authorizationService, userManager)
    {
    }

    [BindProperty]
    public Contact Contact { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
      Contact = await Context.Contact.FirstOrDefaultAsync(m => m.ContactId == id);

      if (Contact == null)
      {
        return NotFound();
      }
      var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, Contact,
                                                 ContactOperations.Delete);
      if (!isAuthorized.Succeeded)
      {
        return Forbid();
      }
      return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
      Contact = await Context.Contact.FindAsync(id);
      var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                   User, Contact,
                                                   ContactOperations.Delete);
      if (!isAuthorized.Succeeded)
      {
        return Forbid();
      }
      if (Contact != null)
      {
        Context.Contact.Remove(Contact);
        await Context.SaveChangesAsync();
      }

      return RedirectToPage("./Index");
    }
  }
}

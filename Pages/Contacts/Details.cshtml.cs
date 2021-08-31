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
  public class DetailsModel : BasePageModel
  {
    public DetailsModel(
    ApplicationDbContext context,
    IAuthorizationService authorizationService,
    UserManager<IdentityUser> userManager)
    : base(context, authorizationService, userManager)
    {
    }

    public Contact Contact { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
      Contact = await Context.Contact.FirstOrDefaultAsync(m => m.ContactId == id);

      if (Contact == null)
      {
        return NotFound();
      }

      var isAuthorized = User.IsInRole(ContactRole.Manager) ||
                         User.IsInRole(ContactRole.Administrator);

      var currentUserId = UserManager.GetUserId(User);

      if (!isAuthorized
          && currentUserId != Contact.OwnerId
          && Contact.Status != ContactStatus.Approved)
      {
        return Forbid();
      }

      return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, ContactStatus status)
    {
      var contact = await Context.Contact.FirstOrDefaultAsync(m => m.ContactId == id);
      if (contact == null)
      {
        return NotFound();
      }

      var contactOperation = (status == ContactStatus.Approved) ? ContactOperations.Approve : ContactOperations.Reject;
      var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, contactOperation);
      if (!isAuthorized.Succeeded)
      {
        return Forbid();
      }
      contact.Status = status;
      Context.Contact.Update(contact);
      await Context.SaveChangesAsync();

      return RedirectToPage("./Index");
    }
  }
}

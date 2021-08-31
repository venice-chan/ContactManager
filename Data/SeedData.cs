using System;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Authorization;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManager.Data
{


  public static class SeedData
  {
    public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
    {
      using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
      {
        var adminId = await EnsureUser(serviceProvider, "admin@contoso.com", testUserPw);
        await EnsureRole(serviceProvider, adminId, ContactRole.Administrator);

        var managerId = await EnsureUser(serviceProvider, "manager@contoso.com", testUserPw);
        await EnsureRole(serviceProvider, managerId, ContactRole.Manager);

        SeedDb(context, adminId);
      }
    }

    public static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userName, string userPassword)
    {
      var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
      var user = await userManager.FindByNameAsync(userName);
      if (user == null)
      {
        user = new IdentityUser
        {
          UserName = userName,
          EmailConfirmed = true,
          Email = userName
        };
        await userManager.CreateAsync(user, userPassword);
      }
      if (user == null)
      {
        throw new Exception("The password is probably not strong enough");
      }
      return user.Id;
    }

    public static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string roleName)
    {
      IdentityResult result = null;
      var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      if (roleManager == null)
      {
        throw new Exception("roleManager is null");
      }
      if (!await roleManager.RoleExistsAsync(roleName))
      {
        result = await roleManager.CreateAsync(new IdentityRole(roleName));
      }

      var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        throw new Exception("User not exist");
      }
      result = await userManager.AddToRoleAsync(user, roleName);
      return result;
    }

    public static void SeedDb(ApplicationDbContext context, string adminId)
    {
      if (context.Contact.Any())
      {
        return;
      }

      context.Contact.AddRange(
          new Contact
          {
            Name = "Debra Garcia",
            Address = "1234 Main St",
            City = "Redmond",
            State = "WA",
            Zip = "10999",
            Email = "debra@example.com",
            Status = ContactStatus.Approved,
            OwnerId = adminId
          },
          new Contact
          {
            Name = "Thorsten Weinrich",
            Address = "5678 1st Ave W",
            City = "Redmond",
            State = "WA",
            Zip = "10999",
            Email = "thorsten@example.com",
            Status = ContactStatus.Approved,
            OwnerId = adminId
          },
           new Contact
           {
             Name = "Yuhong Li",
             Address = "9012 State st",
             City = "Redmond",
             State = "WA",
             Zip = "10999",
             Email = "yuhong@example.com",
             Status = ContactStatus.Approved,
             OwnerId = adminId
           },
           new Contact
           {
             Name = "Jon Orton",
             Address = "3456 Maple St",
             City = "Redmond",
             State = "WA",
             Zip = "10999",
             Email = "jon@example.com",
             Status = ContactStatus.Approved,
             OwnerId = adminId
           },
           new Contact
           {
             Name = "Diliana Alexieva-Bosseva",
             Address = "7890 2nd Ave E",
             City = "Redmond",
             State = "WA",
             Zip = "10999",
             Email = "diliana@example.com",
             Status = ContactStatus.Approved,
             OwnerId = adminId
           }
      );
      context.SaveChanges();
    }
  }
}
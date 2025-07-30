using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Seeding;

public static class ApplicationUserSeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var existingUser = await userManager.FindByEmailAsync("admin@example.com");
        if (existingUser != null) return;

        var user = new ApplicationUser
        {
            FirstName = "Admin",
            LastName = "User",
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "StrongPassword123###");

        if (result.Succeeded)
        {
            var roleId = "d1f488a3-6730-47cb-a0e1-aaa2342a1bc1";
            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await userManager.AddToRoleAsync(user, role.Name);
            }
            else
            {
                // try to get role by name
                role = await roleManager.FindByNameAsync("SuperAdmin");
                if (role != null) await userManager.AddToRoleAsync(user, role.Name);
            }
        }
    }
}
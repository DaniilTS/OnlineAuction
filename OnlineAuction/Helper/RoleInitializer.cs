using OnlineAuction.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace OnlineAuction.Helper
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "admin";
            string adminEmail = "d.tsukrov@gmail.com";
            string password = "admin123456789";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail)==null)
            {
                User admin = new User
                {
                    UserName = adminName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
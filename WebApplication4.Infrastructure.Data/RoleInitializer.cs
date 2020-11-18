using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApplication4.Domain.Core;

namespace WebApplication4.Infrastructure.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<Profile> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            string adminEmail = configuration["Admin:Email"];
            string adminUserName = configuration["Admin:UserName"];
            string adminpassword = configuration["Admin:Password"];
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("employee"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                Profile admin = new Profile { Email = adminEmail, UserName = adminUserName };
                IdentityResult result = await userManager.CreateAsync(admin, adminpassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}

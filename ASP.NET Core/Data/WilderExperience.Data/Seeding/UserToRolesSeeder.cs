namespace WilderExperience.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;

    public class UserToRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserToRoleAsync(userManager);
        }

        private static async Task SeedUserToRoleAsync(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);
            if (user == null)
            {
                var result = await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = GlobalConstants.AdminUsername,
                        Email = GlobalConstants.AdminEmail,
                    },
                    "desiPass");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            user = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);

            await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}

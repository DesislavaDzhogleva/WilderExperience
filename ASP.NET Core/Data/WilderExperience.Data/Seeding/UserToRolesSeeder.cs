namespace WilderExperience.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;

    public class UserToRolesSeeder : ISeeder
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public UserToRolesSeeder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedAdministratorToRoleAsync(userManager, this.configuration);
            await SeedUserToRoleAsync(userManager, this.configuration);
        }

        private static async Task SeedAdministratorToRoleAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);
            if (user == null)
            {
                var result = await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = configuration["Admin:UserName"],
                        Email = configuration["Admin:Email"],
                        EmailConfirmed = true,
                    },
                    configuration["Admin:Password"]);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            user = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);

            //await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedUserToRoleAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByNameAsync(GlobalConstants.UserUsername);
            if (user == null)
            {
                var result = await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = configuration["User:UserName"],
                        Email = configuration["User:Email"],
                        EmailConfirmed = true,
                    },
                    configuration["User:Password"]);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            user = await userManager.FindByNameAsync(GlobalConstants.UserUsername);

            //await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);
        }

    }
}

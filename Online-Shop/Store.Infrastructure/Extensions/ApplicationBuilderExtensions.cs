namespace Store.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Store.Data;
    using Store.Data.Models;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<StoreDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {

                        var role = ModelConstants.AdminRoleName;

                        var roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }

                        var adminEmail = ModelConstants.AdminEmail;
                        var adminUsername = ModelConstants.AdminUsername;
                        var adminPassword = ModelConstants.AdminPassword;

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = adminEmail,
                                Avatar = @"http://thecatapi.com/api/images/get?format=src&type=gif",
                                UserName = adminUsername,
                                FirstName = ModelConstants.AdminFirstName,
                                LastName = ModelConstants.AdminLastName,
                            };

                            await userManager.CreateAsync(adminUser, adminPassword);

                            await userManager.AddToRoleAsync(adminUser, ModelConstants.AdminRoleName);
                        }
                    })
                    .Wait();
            }

            return app;
        }
    }
}

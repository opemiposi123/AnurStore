using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnurStore.Persistence.Context.Seeder
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> SeedToDatabaseAsync(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ContextSeeder.SeedRolesAsync(userManager, roleManager);
                await ContextSeeder.SeedAdminAsync(userManager, roleManager);
                await ContextSeeder.SeedCashierAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                // Log the exception (use an appropriate logging framework)
                //var logger = services.GetRequiredService<ILogger<ApplicationBuilderExtensions>>();
                //logger.LogError(ex, "An error occurred while seeding the database.");
            }

            return app;
        }
    }
}

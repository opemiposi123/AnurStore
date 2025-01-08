using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AnurStore.Persistence.Context.Seeder
{
    public class ContextSeeder
    {
        public static async Task SeedRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Cashier.ToString()));
        }
        public static async Task SeedAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                FirstName = "MahmoodAhmad",
                LastName = "AbdulWaheed",
                Email = "abdulwaheedmahmoodahmad6@gmail.com",
                PhoneNumber = "091653538299",
                Address = "No 4,Idowu Buhari Str Robiyan",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Gender = Gender.Male,
                CreatedBy = "System",
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, Role.Cashier.ToString());
                await userManager.AddToRoleAsync(defaultUser, Role.Admin.ToString());
            }
        }
        public static async Task SeedCashierAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Cashier",
                FirstName = "Hafizoh Ameerah",
                LastName = "AbdulWaheed",
                Email = "ameerah@gmail.com",
                PhoneNumber = "08051550404",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedBy = "Admin",
                Gender = Gender.Female,
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Cashier@123");
                await userManager.AddToRoleAsync(defaultUser, Role.Cashier.ToString());
            }

        }
    }
}

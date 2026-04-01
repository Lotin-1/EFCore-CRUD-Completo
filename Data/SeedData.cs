using Microsoft.AspNetCore.Identity;

namespace TiendaWeb.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string adminRole = "Admin";
            string userRole = "Cliente";

            // crear roles
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!await roleManager.RoleExistsAsync(userRole))
                await roleManager.CreateAsync(new IdentityRole(userRole));

            // crear usuario admin
            string email = "admin@tienda.com";
            string password = "Admin123!";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}
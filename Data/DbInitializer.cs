using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "Admin", "Doctor", "Receptionist" };

            // ✅ 1. Seed Roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"✅ Created role: {role}");
                }
            }

            // ✅ 2. Seed Users

            var users = new List<(string Username, string Email, string Password, string Role)>
            {
                ("admin", "admin@clinic.com", "Admin123!", "Admin"),
                ("dr.yassine", "doctor@clinic.com", "Doctor123!", "Doctor"),
                ("lina", "reception@clinic.com", "Reception123!", "Receptionist")
            };

            foreach (var (username, email, password, role) in users)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser
                    {
                        UserName = username,
                        Email = email,
                        Role = role
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                        Console.WriteLine($"✅ Created {role}: {username} ({email})");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Failed to create {role}: {email}");
                        foreach (var error in result.Errors)
                            Console.WriteLine($" - {error.Description}");
                    }
                }
            }
        }
    }
}

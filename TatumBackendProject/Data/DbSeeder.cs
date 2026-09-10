using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TatumBackendProject.Common.Constants;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context) {
            await context.Database.MigrateAsync();
            await SeedSuperAdminAsync(context);
        }

        private static async Task SeedSuperAdminAsync(AppDbContext context)
        {
            const string email = "speradmin@tatumconnect.com";
            var existinguser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existinguser != null) return;

            var superAdmin = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                //Development password.
                // Change this immediately in a real environment
                PasswordHash = HashPassword("Admin@123456"),
                FirstName = "Tatum",
                LastName = "Super Admin",
                Phone = "+2348000000000",
                Department = "Administration",
                ProfileImageUrl = null,
                StaffId = await GenerateStaffIdAsync(context),
                Role = UserRoles.SuperAdmiin,
                IsActive = true,
                PasswordSetupToken = null,
                PasswordSetupTokenExpiresAt = null,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(superAdmin);

            await context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(password);

            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}

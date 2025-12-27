using Marka.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Marka.Api;

public static class UpdatePassword
{
    public static async Task UpdateTestUserPassword(AppDbContext context)
    {
        var testUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == "testuser@marka.com");

        if (testUser != null && string.IsNullOrEmpty(testUser.PasswordHash))
        {
            testUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123");
            await context.SaveChangesAsync();
            Console.WriteLine("Updated test user password hash");
        }
    }
}

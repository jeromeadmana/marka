using Marka.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marka.Api.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Customers.AnyAsync())
        {
            return; // Data already seeded
        }

        // Create test customer
        var testCustomer = new Customer
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"),
            Name = "Test Company",
            ContactName = "Test Admin",
            ContactEmail = "contact@testcompany.com",
            ContactPhone = "+63-917-123-4567",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Customers.AddAsync(testCustomer);

        // Create sample customer
        var acmeCustomer = new Customer
        {
            Id = Guid.Parse("f1e2d3c4-b5a6-4978-8c9d-0e1f2a3b4c5d"),
            Name = "Acme Corporation",
            ContactName = "John Doe",
            ContactEmail = "john.doe@acme.com",
            ContactPhone = "+63-917-987-6543",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Customers.AddAsync(acmeCustomer);

        // Create SuperAdmin user
        // Password: "password123" (hashed with BCrypt)
        var testUser = new User
        {
            Id = Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"),
            FirstName = "Super",
            LastName = "Admin",
            Email = "admin@marka.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.SuperAdmin,
            CustomerId = testCustomer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Users.AddAsync(testUser);

        // Create CustomerAdmin user for Acme Corporation
        // Password: "acme123" (hashed with BCrypt)
        var acmeAdmin = new User
        {
            Id = Guid.Parse("c4d5e6f7-a8b9-4c0d-1e2f-3a4b5c6d7e8f"),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@acme.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("acme123"),
            Role = UserRole.CustomerAdmin,
            CustomerId = acmeCustomer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Users.AddAsync(acmeAdmin);

        // Create regular User for Acme Corporation
        // Password: "user123" (hashed with BCrypt)
        var acmeUser = new User
        {
            Id = Guid.Parse("d5e6f7a8-b9c0-4d1e-2f3a-4b5c6d7e8f9a"),
            FirstName = "Bob",
            LastName = "Johnson",
            Email = "bob.johnson@acme.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
            Role = UserRole.User,
            CustomerId = acmeCustomer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Users.AddAsync(acmeUser);

        // Create sample markas around Manila
        var markas = new List<MarkaEntity>
        {
            new MarkaEntity
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"),
                Name = "Fort Santiago",
                Description = "Historic walled fortress",
                Latitude = 14.5929,
                Longitude = 120.9738,
                Address = "Intramuros, Manila",
                Category = "Historical",
                Status = "Active",
                CustomerId = testCustomer.Id,
                CreatedByUserId = testUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MarkaEntity
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"),
                Name = "Rizal Monument",
                Description = "National hero monument",
                Latitude = 14.5833,
                Longitude = 120.9789,
                Address = "Rizal Park, Manila",
                Category = "Historical",
                Status = "Active",
                CustomerId = testCustomer.Id,
                CreatedByUserId = testUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MarkaEntity
            {
                Id = Guid.Parse("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"),
                Name = "SM Mall of Asia",
                Description = "Large shopping mall",
                Latitude = 14.5352,
                Longitude = 120.9823,
                Address = "Pasay, Metro Manila",
                Category = "Shopping",
                Status = "Active",
                CustomerId = testCustomer.Id,
                CreatedByUserId = testUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MarkaEntity
            {
                Id = Guid.Parse("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"),
                Name = "Quezon Memorial",
                Description = "Memorial shrine and park",
                Latitude = 14.6527,
                Longitude = 121.0499,
                Address = "Quezon City",
                Category = "Park",
                Status = "Active",
                CustomerId = testCustomer.Id,
                CreatedByUserId = testUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MarkaEntity
            {
                Id = Guid.Parse("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"),
                Name = "BGC High Street",
                Description = "Modern urban district",
                Latitude = 14.5514,
                Longitude = 121.0471,
                Address = "Taguig, Metro Manila",
                Category = "Commercial",
                Status = "Active",
                CustomerId = testCustomer.Id,
                CreatedByUserId = testUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Acme Corporation markas
            new MarkaEntity
            {
                Id = Guid.Parse("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"),
                Name = "Acme HQ",
                Description = "Acme Corporation Headquarters",
                Latitude = 14.5547,
                Longitude = 121.0244,
                Address = "Makati, Metro Manila",
                Category = "Office",
                Status = "Active",
                CustomerId = acmeCustomer.Id,
                CreatedByUserId = acmeAdmin.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MarkaEntity
            {
                Id = Guid.Parse("c9d0e1f2-a3b4-4c5d-6e7f-8a9b0c1d2e3f"),
                Name = "Acme Warehouse",
                Description = "Main distribution center",
                Latitude = 14.5995,
                Longitude = 120.9842,
                Address = "Caloocan, Metro Manila",
                Category = "Warehouse",
                Status = "Active",
                CustomerId = acmeCustomer.Id,
                CreatedByUserId = acmeUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.Markas.AddRangeAsync(markas);
        await context.SaveChangesAsync();

        Console.WriteLine($"Seeded {markas.Count} markas for customers: {testCustomer.Name}, {acmeCustomer.Name}");
    }
}

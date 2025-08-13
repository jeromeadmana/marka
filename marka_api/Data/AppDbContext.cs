using marka_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marka_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}

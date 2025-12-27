using Microsoft.EntityFrameworkCore;
using Marka.Api.Models;

namespace Marka.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets (database tables)
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MarkaEntity> Markas { get; set; }
    public DbSet<MarkaAttribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<CustomRole> CustomRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<MarkaContext> MarkaContexts { get; set; }
    public DbSet<MarkaContextAttribute> MarkaContextAttributes { get; set; }
    public DbSet<AttributeSet> AttributeSets { get; set; }
    public DbSet<AttributeSetAttribute> AttributeSetAttributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Name);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CustomRole)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.CustomRoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Marka configuration
        modelBuilder.Entity<MarkaEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("markas");

            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Active");

            // Latitude and Longitude
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();

            // Indexes
            entity.HasIndex(e => new { e.Latitude, e.Longitude });
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CustomerId);

            // Soft delete query filter
            entity.HasQueryFilter(e => e.DeletedAt == null);

            // Relationships
            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MarkaContext)
                .WithMany(mc => mc.Markas)
                .HasForeignKey(e => e.MarkaContextId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Attribute configuration
        modelBuilder.Entity<MarkaAttribute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Options).HasColumnType("jsonb");
            entity.Property(e => e.ValidationRules).HasColumnType("jsonb");

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // AttributeValue configuration
        modelBuilder.Entity<AttributeValue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Value);

            entity.HasIndex(e => new { e.AttributeId, e.MarkaId }).IsUnique();

            entity.HasOne(e => e.Attribute)
                .WithMany()
                .HasForeignKey(e => e.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Marka)
                .WithMany(m => m.AttributeValues)
                .HasForeignKey(e => e.MarkaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Permission configuration
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Category);
        });

        // CustomRole configuration
        modelBuilder.Entity<CustomRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => new { e.CustomerId, e.Name });

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // RolePermission configuration
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.CustomRoleId, e.PermissionId }).IsUnique();

            entity.HasOne(e => e.CustomRole)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.CustomRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MarkaContext configuration
        modelBuilder.Entity<MarkaContext>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Icon).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.HasIndex(e => new { e.CustomerId, e.Name });

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // MarkaContextAttribute configuration
        modelBuilder.Entity<MarkaContextAttribute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.MarkaContextId, e.MarkaAttributeId }).IsUnique();
            entity.HasIndex(e => new { e.MarkaContextId, e.AttributeOrder });

            entity.HasOne(e => e.MarkaContext)
                .WithMany(mc => mc.MarkaContextAttributes)
                .HasForeignKey(e => e.MarkaContextId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MarkaAttribute)
                .WithMany(a => a.MarkaContextAttributes)
                .HasForeignKey(e => e.MarkaAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AttributeSet configuration
        modelBuilder.Entity<AttributeSet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => new { e.CustomerId, e.Name });

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // AttributeSetAttribute configuration
        modelBuilder.Entity<AttributeSetAttribute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.AttributeSetId, e.MarkaAttributeId }).IsUnique();
            entity.HasIndex(e => new { e.AttributeSetId, e.AttributeOrder });

            entity.HasOne(e => e.AttributeSet)
                .WithMany(aset => aset.AttributeSetAttributes)
                .HasForeignKey(e => e.AttributeSetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MarkaAttribute)
                .WithMany(a => a.AttributeSetAttributes)
                .HasForeignKey(e => e.MarkaAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

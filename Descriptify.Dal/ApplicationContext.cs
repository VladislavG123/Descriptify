using Descriptify.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Descriptify.Dal;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreatedAsync();
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    public override int SaveChanges()
    {
        ChangeTracker.DetectChanges();
        foreach (var entry in ChangeTracker.Entries()
                     .Where(x => x.State == EntityState.Deleted))
        {
            if (entry.Entity is BaseEntity entity)
            {
                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<CategoryEntity>().HasQueryFilter(x => !x.IsDeleted);
    }
}
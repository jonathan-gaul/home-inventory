using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assets.Data.Context;

public class AssetsDbContext(DbContextOptions<AssetsDbContext> options) : DbContext(options)
{
    public DbSet<AssetEntity> Assets => Set<AssetEntity>();
    public DbSet<LocationEntity> Locations => Set<LocationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssetEntity>()
            .ToTable("Assets", b => b.IsTemporal());

        modelBuilder.Entity<LocationEntity>(e =>
        {
            e.ToTable("Locations", b => b.IsTemporal());

            e.HasOne(l => l.ParentLocation)
                .WithMany(l => l.ChildLocations)
                .HasForeignKey(l => l.ParentLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

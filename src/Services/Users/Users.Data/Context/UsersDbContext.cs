using Microsoft.EntityFrameworkCore;
using Users.Data.Entities;

namespace Users.Data.Context;

public class UsersDbContext : DbContext
{
    public DbSet<Organisation> Organisations => Set<Organisation>();
    public DbSet<OrganisationMember> OrganisationMembers => Set<OrganisationMember>();

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrganisationMember>(entity =>
            {
                entity.HasKey(om => new { om.OrganisationId, om.UserId });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Role)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.JoinedAt)
                    .IsRequired();

                entity.HasIndex(e => e.OrganisationId);
                entity.HasIndex(e => e.UserId);
            });

        modelBuilder.Entity<Organisation>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
    }


}
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Assets.Data.Context;

public class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
    : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.LocationId)
                .IsRequired();

            entity.Property(e => e.PurchaseCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.CurrentValue)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.SerialNumber)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.OrganisationId)
                .IsRequired();

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.SerialNumber);
            entity.HasIndex(e => e.ModelNumber);
            entity.HasIndex(e => e.OrganisationId);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.Property(e => e.LastUpdatedAt)
                .IsRequired();

            entity.Property(e => e.OrganisationId)
                .IsRequired();

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.ParentLocationId);
            entity.HasIndex(e => e.OrganisationId);
        });
    }
}

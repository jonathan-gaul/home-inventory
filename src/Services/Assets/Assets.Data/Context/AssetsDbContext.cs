using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Context;

public class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
    : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();


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

            entity.Property(e => e.PurchaseCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.CurrentValue)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.SerialNumber)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.SerialNumber);
            entity.HasIndex(e => e.ModelNumber);
        });
    }
}

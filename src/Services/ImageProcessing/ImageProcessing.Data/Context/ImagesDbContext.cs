using ImageProcessing.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessing.Data.Context;

public class ImagesDbContext : DbContext
{
    public ImagesDbContext(DbContextOptions<ImagesDbContext> options)
        : base(options)
    {
    }
    public DbSet<Image> Images { get; set; } = null!;
}

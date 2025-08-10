using InstaShare.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstaShare.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<FileEntity> Files => Set<FileEntity>();

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileEntity>(e =>
        {
            e.Property(p => p.Status).HasConversion<byte>();
            e.HasIndex(p => new { p.UserId, p.HashSha256 }).IsUnique();
            e.HasIndex(p => p.UserId);
            e.HasIndex(p => p.Status);
        });
    }
}
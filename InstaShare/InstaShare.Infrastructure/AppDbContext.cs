using InstaShare.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstaShare.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<FileEntity> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileEntity>();

        modelBuilder.Entity<FileEntity>()
            .HasData(
                new FileEntity
                {
                    Id = new Guid("ff0c022e-1aff-4ad8-2231-08db0378ac98"),
                    Name = "Default File",
                    Status = "upload",
                    Size = 1024
                }
            );
    }

    // Basic Code to add more sample data
    // It creates 500 Random Files
    public void SeedInitialData()
    {
        var files = new List<FileEntity>();

        for (int i = 1; i <= 500; ++i)
        {
            var file = new FileEntity
            {
                Id = Guid.NewGuid(),
                Name = $"File {i}",
                Status = "upload",
                Size = Random.Shared.Next(1, 51200),
            };

            files.Add(file);
        }

        Files.AddRange(files);
    }
}
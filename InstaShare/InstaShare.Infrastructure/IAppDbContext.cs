using InstaShare.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstaShare.Infrastructure;

public interface IAppDbContext
{
    DbSet<FileEntity> Files { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
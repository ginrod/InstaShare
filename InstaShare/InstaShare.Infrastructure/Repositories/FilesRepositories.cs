using InstaShare.Application.Entities;
using InstaShare.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Infrastructure.Repositories
{
    public class FilesRepository : EntityRepository<FileEntity, Guid>, IFilesRepository
    {
        private readonly FilesDataContext _context;

        public FilesRepository(FilesDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<FileEntity>, int)> SearchFilesAsync(string searchTerm, int page, int pageSize)
        {
            var query = _dbSet
                .Where(p => (p.Name ?? "").Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(p => p.Name);

            var totalRecords = await query.CountAsync();

            var Files = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Files, totalRecords);
        }
    }
}

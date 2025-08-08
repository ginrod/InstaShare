using InstaShare.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Repositories.Interfaces
{
    public interface IFilesRepository : IEntitiesRepository<FileEntity, Guid>
    {
        Task<(IEnumerable<FileEntity>, int)> SearchFilesAsync(string searchTerm, int page, int pageSize);
    }
}

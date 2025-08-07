using InstaShare.Application.Entities;
using InstaShare.Application.Repositories.Interfaces;
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
    }
}

using InstaShare.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Respositories.Interfaces
{
    public interface IFilesRepository : IEntitiesRepository<FileEntity, Guid>
    {

    }
}

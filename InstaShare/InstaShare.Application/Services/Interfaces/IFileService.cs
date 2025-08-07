using InstaShare.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileEntity> GetOfficeByIdAsync(Guid id);
        Task<IEnumerable<FileEntity>> GetAllOfficesAsync();
        Task AddOfficeAsync(FileEntity contact);
        Task UpdateOfficeAsync(FileEntity contact);
        Task DeleteOfficeAsync(Guid id);
    }
}

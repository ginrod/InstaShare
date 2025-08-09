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
        Task<FileEntity> GetFileByIdAsync(Guid id);
        Task<(IEnumerable<FileEntity>, int)> SearchAllFilesAsync(int page, int pageSize);
        Task AddFileAsync(FileEntity contact);
        Task UpdateFileAsync(FileEntity contact);
        Task DeleteFileAsync(Guid id);
        Task<(IEnumerable<FileEntity>, int)> SearchFilesAsync(string searchTerm, int page, int pageSize);

    }
}

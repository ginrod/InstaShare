using InstaShare.Application.Entities;
using InstaShare.Application.Repositories.Interfaces;
using InstaShare.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Services
{
    public class FileService : IFileService
    {
        private IFilesRepository _filesRepository;

        public FileService(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        public async Task AddFileAsync(FileEntity file)
        {
            await _filesRepository.AddAsync(file);
        }

        public async Task DeleteFileAsync(Guid id)
        {
            await _filesRepository.DeleteAsync(id);
        }

        public async Task<(IEnumerable<FileEntity>, int)> SearchAllFilesAsync(int page, int pageSize)
        {
            return await _filesRepository.SearchAllFilesAsync(page, pageSize);
        }

        public async Task<FileEntity> GetFileByIdAsync(Guid id)
        {
            return await _filesRepository.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<FileEntity>, int)> SearchFilesAsync(string searchTerm, int page, int pageSize)
        {
            return await _filesRepository.SearchFilesAsync(searchTerm, page, pageSize);
        }

        public async Task UpdateFileAsync(FileEntity file)
        {
            await _filesRepository.UpdateAsync(file);
        }
    }
}

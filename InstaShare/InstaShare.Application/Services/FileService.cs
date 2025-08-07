using InstaShare.Application.Entities;
using InstaShare.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Services
{
    public class FileService : IFilesRepository
    {
        private IFilesRepository _filesRepository;

        public FileService(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        public async Task AddAsync(FileEntity entity)
        {
            await _filesRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _filesRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FileEntity>> GetAllAsync()
        {
            return await _filesRepository.GetAllAsync();
        }

        public async Task<FileEntity> GetByIdAsync(Guid id)
        {
            return await _filesRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(FileEntity entity)
        {
            await _filesRepository.UpdateAsync(entity);
        }
    }
}

using Asp.Versioning;
using InstaShare.Application.Dtos;
using InstaShare.Application.Entities;
using InstaShare.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace InstaShare.API.Controllers
{
    [Route("api/v{version:apiVersion}/Files")]
    [ApiExplorerSettings(GroupName = "Files")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Retrieves a list of Files.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A list of Files</returns>
        /// <response code="200">Files retrieved successfully.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("getAllFiles")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FileEntity>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [HttpGet(Name = "FilesController")]
        public async Task<IActionResult> GetFiles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (files, totalRecords) = await _fileService.SearchAllFilesAsync(page, pageSize);

            return Ok(new ApiResponse<IEnumerable<FileEntity>>
                (success: true, "Files retrieved successfully", files, totalRecords)
            );
        }

        /// <summary>
        /// Retrieves a list of Files.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A list of Files</returns>
        /// <response code="200">Files retrieved successfully.</response>
        /// <response code="400">If the search term is empty or invalid.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("searchFiles")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FileEntity>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [HttpGet(Name = "FilesController")]
        public async Task<IActionResult> SearchFiles([FromQuery] string term, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new ApiResponse<object>(false, "Search term cannot be empty.", null));
            }

            var (files, totalRecords) = await _fileService.SearchFilesAsync(term, page, pageSize);

            return Ok(new ApiResponse<IEnumerable<FileEntity>>
                (success: true, "Files retrieved successfully", files, totalRecords)
            );
        }
    }
}

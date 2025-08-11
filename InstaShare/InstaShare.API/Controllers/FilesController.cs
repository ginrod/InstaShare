using Asp.Versioning;
using InstaShare.API.Services.Interfaces;
using InstaShare.Application.Dtos;
using InstaShare.Application.Entities;
using InstaShare.Application.Services.Interfaces;
using InstaShare.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace InstaShare.API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Files")]
    [ApiExplorerSettings(GroupName = "Files")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IBlobStorage _blob;
        private readonly IZipQueue _queue;
        private readonly AppDbContext _db;

        public FilesController(IFileService fileService, AppDbContext db, IBlobStorage blob, IZipQueue queue)
        {
            _fileService = fileService;
            _db = db;
            _blob = blob;
            _queue = queue;
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

        [HttpPost("uploadFile")] // multipart
        [RequestSizeLimit(1024L * 1024L * 200)] // 200 MB ex.
        public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0) return BadRequest("Empty file");

            // calculate SHA256
            string hashHex;
            await using (var s = file.OpenReadStream())
            {
                using var sha = SHA256.Create();
                var h = await sha.ComputeHashAsync(s, ct);
                hashHex = Convert.ToHexString(h).ToLowerInvariant();
            }

            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("oid")?.Value ?? "unknown";
            var id = Guid.NewGuid();
            var blobName = $"{userId}/{id}-{Path.GetFileName(file.FileName)}";

            // upload to blob
            await using var stream = file.OpenReadStream();
            await _blob.UploadAsync("original", blobName, stream, file.ContentType, ct);

            var rec = new FileEntity
            {
                Id = id,
                UserId = userId,
                Name = file.FileName,
                ContentType = file.ContentType ?? "application/octet-stream",
                SizeBytes = file.Length,
                HashSha256 = hashHex,
                OriginalBlob = $"original/{blobName}",
                Status = FileStatus.Uploaded
            };

            _db.Files.Add(rec);
            await _db.SaveChangesAsync(ct);

            await _queue.EnqueueAsync(new ZipJob(rec.Id), ct);

            return Accepted(new { id = rec.Id });
        }

        [HttpPatch("{id:guid}/name")]
        public async Task<IActionResult> Rename(Guid id, [FromBody] string newName, CancellationToken ct)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("oid")?.Value ?? "unknown";
            var rec = await _db.Files.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, ct);
            if (rec is null) return NotFound();

            rec.Name = newName;
            rec.UpdatedTime = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpGet("{id:guid}/download")]
        public async Task<IActionResult> Download(Guid id, CancellationToken ct)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("oid")?.Value ?? "unknown";
            var rec = await _db.Files.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, ct);
            if (rec is null) return NotFound();
            if (rec.Status != FileStatus.Zipped || string.IsNullOrEmpty(rec.ZipBlob)) return BadRequest("Not ready");

            var parts = rec.ZipBlob.Split('/', 2);
            var sas = await _blob.GetReadSasUriAsync(parts[0], parts[1], TimeSpan.FromMinutes(10));
            return Ok(new { url = sas });
        }
    }
}

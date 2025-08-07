using Asp.Versioning;
using InstaShare.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InstaShare.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{

    private readonly ILogger<FilesController> _logger;

    public FilesController(ILogger<FilesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of Files.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>A list of Offices</returns>
    /// <response code="200">Offices retrieved successfully.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("getFiles")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FileEntity>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    [Produces("application/json")]
    [MapToApiVersion("1.0")]
    [HttpGet(Name = "FilesController")]
    public IEnumerable<FileEntity> Get()
    {
        return Enumerable.Range(1, 500).Select(index => new FileEntity
        {
            Name = $"File {index}",
            Status = "uploaded",
            Size = Random.Shared.Next(1, 51200),
        })
        .ToArray();
    }
}

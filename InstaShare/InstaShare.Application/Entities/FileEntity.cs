using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Entities
{
    public enum FileStatus : byte
    {
        Uploaded = 0,
        Queued = 1,
        Processing = 2,
        Zipped = 3,
        Failed = 4
    }

    public class FileEntity : Entity
    {
        [Required, MaxLength(100)]
        public string UserId { get; set; } = "";
        
        [Required, MaxLength(260)]
        public string? Name { get; set; } = null;

        [Required, MaxLength(100)]
        public string ContentType { get; set; } = "application/octet-stream";

        [Range(0, long.MaxValue)]
        public long SizeBytes { get; set; }

        [Required, StringLength(64)]
        public string HashSha256 { get; set; } = ""; // hex lower

        [Required, MaxLength(400)]
        public string OriginalBlob { get; set; } = default!; // container/blobName

        [MaxLength(400)]
        public string? ZipBlob { get; set; }

        [Required]
        public FileStatus Status { get; set; } = FileStatus.Uploaded;

        [Timestamp]                                                
        public byte[] RowVersion { get; set; } = default!; // EF Core concurrency token
    }
}

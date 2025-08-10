using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.API.Services.Interfaces
{
    public interface IBlobStorage
    {
        Task UploadAsync(string container, string blobName, Stream content, string contentType, CancellationToken ct);
        Task<Stream> DownloadAsync(string container, string blobName, CancellationToken ct);
        Task<string> GetReadSasUriAsync(string container, string blobName, TimeSpan ttl);
    }
}

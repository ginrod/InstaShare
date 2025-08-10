using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using InstaShare.API.Services.Interfaces;
using InstaShare.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaShare.Application.Services
{
    public class AzureBlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _svc;
        public AzureBlobStorage(BlobServiceClient svc) => _svc = svc;

        public async Task UploadAsync(string container, string blobName, Stream content, string contentType, CancellationToken ct)
        {
            var c = _svc.GetBlobContainerClient(container);
            await c.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
            var b = c.GetBlobClient(blobName);
            await b.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        }

        public async Task<Stream> DownloadAsync(string container, string blobName, CancellationToken ct)
        {
            var b = _svc.GetBlobContainerClient(container).GetBlobClient(blobName);
            var resp = await b.DownloadAsync(ct);
            return resp.Value.Content;
        }

        public Task<string> GetReadSasUriAsync(string container, string blobName, TimeSpan ttl)
        {
            var b = _svc.GetBlobContainerClient(container).GetBlobClient(blobName);
            var sas = b.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(ttl));
            return Task.FromResult(sas.ToString());
        }
    }
}

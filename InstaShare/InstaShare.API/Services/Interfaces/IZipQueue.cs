namespace InstaShare.API.Services.Interfaces
{
    public record ZipJob(Guid FileId);

    public interface IZipQueue
    {
        Task EnqueueAsync(ZipJob job, CancellationToken ct);
        Task<ZipJob?> DequeueAsync(CancellationToken ct);
    }

}

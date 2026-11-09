using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace RaceDay.API.Services;
public class BlobService
{
    private readonly BlobContainerClient _container;
    public BlobService(IConfiguration config)
    {
        var conn = config["AzureBlob:ConnectionString"] ?? "UseDevelopmentStorage=true";
        _container = new BlobContainerClient(conn, "raceday-images");
    }
    public async Task<string> UploadAsync(IFormFile file)
    {
        try
        {
            await _container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            await _container.UploadBlobAsync(name, file.OpenReadStream());
            return _container.GetBlobClient(name).Uri.ToString();
        }
        catch { return "https://via.placeholder.com/600x300?text=RaceDay"; }
    }
}
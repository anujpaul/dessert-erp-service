using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DessertERP.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DessertERP.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage implementation of IBlobStorageService.
///
/// Configuration (appsettings.json or Azure App Configuration):
///   "AzureStorage": {
///     "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
///   }
///
/// For local development, use Azurite (Azure Storage Emulator):
///   "AzureStorage": {
///     "ConnectionString": "UseDevelopmentStorage=true"
///   }
/// </summary>
public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(IConfiguration config,
        ILogger<AzureBlobStorageService> logger)
    {
        _logger = logger;
        var connStr = config["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException(
               "AzureStorage:ConnectionString is not configured. " +
               "For local development, set it to 'UseDevelopmentStorage=true' (requires Azurite). " +
               "For Azure, set the storage account connection string.");
        _client = new BlobServiceClient(connStr);
    }

    public async Task<List<BlobFileInfo>> ListFilesAsync(string containerName, string prefix,
        string extension, CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        
        var results   = new List<BlobFileInfo>();

        await foreach (var item in container.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
        {
            if (!item.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) continue;
            results.Add(new BlobFileInfo(
                item.Name,
                Path.GetFileName(item.Name),
                item.Properties.ContentLength ?? 0,
                item.Properties.LastModified));
        }
        return results;
    }

    public async Task<string> DownloadToTempFileAsync(string containerName, string blobName,
        CancellationToken ct = default)
    {
        var container  = _client.GetBlobContainerClient(containerName);
        var blobClient = container.GetBlobClient(blobName);

        var ext      = Path.GetExtension(blobName);
        var tempPath = Path.Combine(Path.GetTempPath(), $"dessert_erp_{Guid.NewGuid()}{ext}");

        _logger.LogDebug("Downloading blob {BlobName} to {TempPath}", blobName, tempPath);
        await blobClient.DownloadToAsync(tempPath, ct);
        return tempPath;
    }

    public async Task UploadAsync(string containerName, string blobName, byte[] data,
        string contentType = "application/octet-stream", CancellationToken ct = default)
    {
        var container  = _client.GetBlobContainerClient(containerName);
        var blobClient = container.GetBlobClient(blobName);

        using var ms = new MemoryStream(data);
        await blobClient.UploadAsync(ms, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        _logger.LogDebug("Uploaded {Bytes} bytes to blob {BlobName}", data.Length, blobName);
    }

    public async Task MoveAsync(string containerName, string sourceBlobName, string destBlobName,
        CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        var src  = container.GetBlobClient(sourceBlobName);
        var dest = container.GetBlobClient(destBlobName);

        // Copy
        var copyOp = await dest.StartCopyFromUriAsync(src.Uri, cancellationToken: ct);
        await copyOp.WaitForCompletionAsync(ct);

        // Delete source
        await src.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogDebug("Moved blob {Source} → {Dest}", sourceBlobName, destBlobName);
    }

    public async Task DeleteAsync(string containerName, string blobName,
        CancellationToken ct = default)
    {
        var container  = _client.GetBlobContainerClient(containerName);
        var blobClient = container.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task EnsureContainerExistsAsync(string containerName,
        CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        var created   = await container.CreateIfNotExistsAsync(
            PublicAccessType.None, cancellationToken: ct);
        if (created?.Value != null)
            _logger.LogInformation("Created Azure Blob container '{Container}'", containerName);
    }
}

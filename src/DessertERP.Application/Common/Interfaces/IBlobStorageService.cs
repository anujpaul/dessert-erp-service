namespace DessertERP.Application.Common.Interfaces;

/// <summary>
/// Abstraction over cloud blob storage (Azure Blob Storage or Google Cloud Storage).
/// All paths use forward-slash prefix notation, e.g. "sales-orders/inbox/".
/// </summary>
public interface IBlobStorageService
{
    /// <summary>List blobs in a container under the given prefix that match the extension.</summary>
    Task<List<BlobFileInfo>> ListFilesAsync(string containerName, string prefix,
        string extension, CancellationToken ct = default);

    /// <summary>Download a blob to a local temp file. Returns the temp file path.</summary>
    Task<string> DownloadToTempFileAsync(string containerName, string blobName,
        CancellationToken ct = default);

    /// <summary>Upload bytes to a blob.</summary>
    Task UploadAsync(string containerName, string blobName, byte[] data,
        string contentType = "application/octet-stream", CancellationToken ct = default);

    /// <summary>Move a blob within the same container (copy + delete).</summary>
    Task MoveAsync(string containerName, string sourceBlobName, string destBlobName,
        CancellationToken ct = default);

    /// <summary>Delete a blob.</summary>
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);

    /// <summary>Check whether a container exists and create it if not.</summary>
    Task EnsureContainerExistsAsync(string containerName, CancellationToken ct = default);
}

public record BlobFileInfo(
    string BlobName,        // full path within container, e.g. "sales-orders/inbox/orders.xml"
    string FileName,        // just the file name, e.g. "orders.xml"
    long   SizeBytes,
    DateTimeOffset? LastModified
);

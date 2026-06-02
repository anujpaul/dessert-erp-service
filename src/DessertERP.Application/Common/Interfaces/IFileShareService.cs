namespace DessertERP.Application.Common.Interfaces;

/// <summary>
/// Abstracts file storage for batch jobs — implemented by AzureFileShareService.
/// Paths are share-relative, e.g. "imports/sales-orders/inbox".
/// </summary>
public interface IFileShareService
{
    Task<List<string>> ListFilesAsync(string sharePath, string extension, CancellationToken ct = default);
    Task<byte[]> DownloadAsync(string sharePath, string fileName, CancellationToken ct = default);
    Task UploadAsync(string sharePath, string fileName, byte[] data, CancellationToken ct = default);
    Task MoveAsync(string srcPath, string srcFile, string destPath, string destFile, CancellationToken ct = default);
    Task EnsureDirectoryExistsAsync(string sharePath, CancellationToken ct = default);
}

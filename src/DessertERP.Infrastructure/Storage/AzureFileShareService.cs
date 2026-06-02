using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using DessertERP.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DessertERP.Infrastructure.Storage;

/// <summary>
/// Reads and writes files to an Azure File Share over HTTPS.
/// Works from any environment (Azure App Service, local dev, etc.) — no SMB/port 445 needed.
///
/// Configuration:
///   "AzureStorage": {
///     "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net",
///     "FileShareName": "erp-files"   (defaults to "erp-files")
///   }
/// </summary>
public class AzureFileShareService : IFileShareService
{
    private readonly ShareClient _share;
    private readonly ILogger<AzureFileShareService> _logger;

    public AzureFileShareService(IConfiguration config, ILogger<AzureFileShareService> logger)
    {
        _logger = logger;
        var connStr   = config["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");
        var shareName = config["AzureStorage:FileShareName"] ?? "erp-files";
        _share = new ShareClient(connStr, shareName);
    }

    /// <summary>Lists files in a share directory with the given extension.</summary>
    public async Task<List<string>> ListFilesAsync(string sharePath, string extension, CancellationToken ct = default)
    {
        var dir = GetDirectory(sharePath);
        var results = new List<string>();

        await foreach (var item in dir.GetFilesAndDirectoriesAsync(cancellationToken: ct))
        {
            if (item.IsDirectory) continue;
            if (!item.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) continue;
            results.Add(item.Name);
        }
        return results;
    }

    /// <summary>Downloads a file from the share into a byte array.</summary>
    public async Task<byte[]> DownloadAsync(string sharePath, string fileName, CancellationToken ct = default)
    {
        var file = GetDirectory(sharePath).GetFileClient(fileName);
        var response = await file.DownloadAsync(cancellationToken: ct);
        using var ms = new MemoryStream();
        await response.Value.Content.CopyToAsync(ms, ct);
        return ms.ToArray();
    }

    /// <summary>Uploads bytes to a file in the share, creating intermediate directories as needed.</summary>
    public async Task UploadAsync(string sharePath, string fileName, byte[] data, CancellationToken ct = default)
    {
        await EnsureDirectoryExistsAsync(sharePath, ct);
        var file = GetDirectory(sharePath).GetFileClient(fileName);
        using var ms = new MemoryStream(data);
        await file.CreateAsync(ms.Length, cancellationToken: ct);
        await file.UploadAsync(ms, cancellationToken: ct);
        _logger.LogDebug("Uploaded {Bytes} bytes to Azure Files: {Path}/{File}", data.Length, sharePath, fileName);
    }

    /// <summary>Moves a file within the share (copy + delete).</summary>
    public async Task MoveAsync(string srcPath, string srcFile, string destPath, string destFile, CancellationToken ct = default)
    {
        var src  = GetDirectory(srcPath).GetFileClient(srcFile);
        await EnsureDirectoryExistsAsync(destPath, ct);
        var dest = GetDirectory(destPath).GetFileClient(destFile);

        // Azure Files doesn't have a native move — copy then delete
        var srcUri = src.Uri;
        await dest.StartCopyAsync(srcUri, cancellationToken: ct);

        // Wait for copy to complete
        ShareFileProperties props;
        do
        {
            await Task.Delay(200, ct);
            props = await dest.GetPropertiesAsync(cancellationToken: ct);
        } while (props.CopyStatus == CopyStatus.Pending);

        await src.DeleteAsync(cancellationToken: ct);
        _logger.LogDebug("Moved Azure Files: {Src}/{SrcFile} → {Dest}/{DestFile}", srcPath, srcFile, destPath, destFile);
    }

    /// <summary>Creates all directories in the path if they don't exist.</summary>
    public async Task EnsureDirectoryExistsAsync(string sharePath, CancellationToken ct = default)
    {
        // Split path and create each segment
        var parts = sharePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var current = _share.GetRootDirectoryClient();

        foreach (var part in parts)
        {
            current = current.GetSubdirectoryClient(part);
            await current.CreateIfNotExistsAsync(cancellationToken: ct);
        }
    }

    private ShareDirectoryClient GetDirectory(string sharePath)
    {
        var parts = sharePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var dir = _share.GetRootDirectoryClient();
        foreach (var part in parts)
            dir = dir.GetSubdirectoryClient(part);
        return dir;
    }
}

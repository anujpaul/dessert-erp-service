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
    private readonly ShareClient? _share;
    private readonly ILogger<AzureFileShareService> _logger;
    private readonly bool _configured;

    public AzureFileShareService(IConfiguration config, ILogger<AzureFileShareService> logger)
    {
        _logger = logger;

        var shareName  = config["AzureStorage:FileShareName"] ?? "erp-files";

        // Strategy 1: explicit FileEndpoint + SAS token (most reliable for SAS auth)
        //   AzureStorage__FileEndpoint  = https://desserterp.file.core.windows.net/
        //   AzureStorage__SasToken      = sv=2026-...&sig=...
        var fileEndpoint = config["AzureStorage:FileEndpoint"];
        var sasToken     = config["AzureStorage:SasToken"];

        // Strategy 2: full connection string (works for AccountKey format)
        //   AzureStorage__ConnectionString = DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...
        var connStr = config["AzureStorage:ConnectionString"];

        try
        {
            if (!string.IsNullOrWhiteSpace(fileEndpoint) && !string.IsNullOrWhiteSpace(sasToken))
            {
                // Build the share URI directly: https://<account>.file.core.windows.net/<share>?<sas>
                var token    = sasToken.TrimStart('?');
                var shareUri = new Uri($"{fileEndpoint.TrimEnd('/')}/{shareName}?{token}");
                _share = new ShareClient(shareUri);
                _configured = true;
                _logger.LogInformation("AzureFileShareService initialised via SAS URI — share: {Share}", shareName);
            }
            else if (!string.IsNullOrWhiteSpace(connStr))
            {
                var serviceClient = new ShareServiceClient(connStr);
                _share = serviceClient.GetShareClient(shareName);
                _configured = true;
                _logger.LogInformation("AzureFileShareService initialised via connection string — share: {Share}", shareName);
            }
            else
            {
                _logger.LogWarning(
                    "AzureStorage not configured — AzureFileShareService running in no-op mode. " +
                    "Set AzureStorage:FileEndpoint + AzureStorage:SasToken, or AzureStorage:ConnectionString.");
                _configured = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                "AzureFileShareService: failed to initialise — running in no-op mode. Error: {Message}", ex.Message);
            _configured = false;
        }
    }

    /// <summary>Returns true if Azure File Share is configured; logs and returns false otherwise.</summary>
    private bool IsReady(string operation)
    {
        if (_configured) return true;
        _logger.LogWarning("AzureFileShareService: skipping '{Operation}' — no connection string configured.", operation);
        return false;
    }

    /// <summary>Lists files in a share directory with the given extension.</summary>
    public async Task<List<string>> ListFilesAsync(string sharePath, string extension, CancellationToken ct = default)
    {
        if (!IsReady(nameof(ListFilesAsync))) return [];
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
        if (!IsReady(nameof(DownloadAsync))) return [];
        var file = GetDirectory(sharePath).GetFileClient(fileName);
        var response = await file.DownloadAsync(cancellationToken: ct);
        using var ms = new MemoryStream();
        await response.Value.Content.CopyToAsync(ms, ct);
        return ms.ToArray();
    }

    /// <summary>Uploads bytes to a file in the share, creating intermediate directories as needed.</summary>
    public async Task UploadAsync(string sharePath, string fileName, byte[] data, CancellationToken ct = default)
    {
        if (!IsReady(nameof(UploadAsync))) return;
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
        if (!IsReady(nameof(MoveAsync))) return;
        var src  = GetDirectory(srcPath).GetFileClient(srcFile);
        await EnsureDirectoryExistsAsync(destPath, ct);
        var dest = GetDirectory(destPath).GetFileClient(destFile);

        // Azure Files doesn't have a native move — copy then delete
        await dest.StartCopyAsync(src.Uri, cancellationToken: ct);

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
        if (!IsReady(nameof(EnsureDirectoryExistsAsync))) return;
        var parts = sharePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var current = _share!.GetRootDirectoryClient();
        foreach (var part in parts)
        {
            current = current.GetSubdirectoryClient(part);
            await current.CreateIfNotExistsAsync(cancellationToken: ct);
        }
    }

    private ShareDirectoryClient GetDirectory(string sharePath)
    {
        var parts = sharePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var dir = _share!.GetRootDirectoryClient();
        foreach (var part in parts)
            dir = dir.GetSubdirectoryClient(part);
        return dir;
    }
}

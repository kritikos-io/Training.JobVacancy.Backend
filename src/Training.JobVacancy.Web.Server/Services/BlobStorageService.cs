namespace Adaptit.Training.JobVacancy.Web.Server.Services;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Web.Server.Options;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

using Microsoft.Extensions.Options;

public class BlobStorageService
{
  private readonly BlobServiceClient _blobServiceClient;
  private readonly ILogger<BlobStorageService> _logger;
  private readonly string _containerName;
  private readonly int _sasValidForMinutes;

  public BlobStorageService(BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger, IOptions<BlobStorageOptions> options)
  {
    _blobServiceClient = blobServiceClient;
    _logger = logger;
    _containerName = options.Value.ContainerName;
    _sasValidForMinutes = options.Value.SasValidForMinutes;
  }

  public async Task<Uri?> UploadFileAsync(Stream fileStream, string fileName, Dictionary<string, string> tags, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync(cancellationToken))
    {
      _logger.LogBlobContainerDoesNotExist(_containerName);

      return null;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var blobHttpHeaders = new BlobHttpHeaders { ContentType = "application/pdf" };

    var blobUploadOptions = new BlobUploadOptions
    {
      HttpHeaders = blobHttpHeaders,
      Tags = tags
    };

    try
    {
      await blobClient.UploadAsync(fileStream, blobUploadOptions, cancellationToken);

      return blobClient.Uri;
    }
    catch(RequestFailedException)
    {
      _logger.LogFailedToUploadFile(fileName);

      return null;
    }
  }

  public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync(cancellationToken))
    {
      _logger.LogBlobContainerDoesNotExist(_containerName);

      return false;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var isDeleted = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

    return isDeleted;
  }

  public async Task<bool> DeleteAllResumesAsync(Guid userId, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync(cancellationToken))
    {
      _logger.LogBlobContainerDoesNotExist(_containerName);

      return false;
    }

    var query = $"UserId = '{userId}'";

    var blobBatchClient = blobContainerClient.GetBlobBatchClient();

    try
    {
      var blobItems = blobContainerClient.FindBlobsByTagsAsync(query, cancellationToken);
      var blobUrisToDelete = new List<Uri>();

      await foreach (var blobItem in blobItems)
      {
        var blobClient = blobContainerClient.GetBlobClient(blobItem.BlobName);
        blobUrisToDelete.Add(blobClient.Uri);
      }

      if (blobUrisToDelete.Count is > 0 and <= 256)
      {
        await blobBatchClient.DeleteBlobsAsync(blobUrisToDelete.ToArray(), cancellationToken: cancellationToken);
      }
      else
      {
        for (var i = 0; i < blobUrisToDelete.Count; i += 256)
        {
          var chunk = blobUrisToDelete.Skip(i).Take(256).ToList();
          await blobBatchClient.DeleteBlobsAsync(chunk.ToArray(), cancellationToken: cancellationToken);
        }
      }

      return true;
    }
    catch (RequestFailedException ex)
    {
      _logger.LogAzureRequestFailed(ex.Message);

      return false;
    }
  }

  public async Task<Uri?> GetReadOnlySasUrlAsync(string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync(cancellationToken))
    {
      _logger.LogBlobContainerDoesNotExist(_containerName);

      return null;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var sasBuilder = new BlobSasBuilder
    {
      BlobContainerName = _containerName,
      BlobName = fileName,
      Resource = "b",
      ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(_sasValidForMinutes)
    };

    sasBuilder.SetPermissions(BlobSasPermissions.Read);

    var sasUri = blobClient.GenerateSasUri(sasBuilder);
    return sasUri;
  }

}

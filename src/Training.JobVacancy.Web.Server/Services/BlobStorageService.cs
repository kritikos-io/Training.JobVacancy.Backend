namespace Adaptit.Training.JobVacancy.Web.Server.Services;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Web.Server.Options;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

using Microsoft.Extensions.Options;

public class BlobStorageService
{
  private readonly BlobServiceClient _blobServiceClient;
  private readonly ILogger<BlobStorageService> _logger;
  private readonly string _containerName;
  private readonly int _sasValidForMinutes;

  public BlobStorageService(ILogger<BlobStorageService> logger, IOptions<BlobStorageOptions> options)
  {
    _logger = logger;
    _blobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
    _containerName = options.Value.ContainerName;
    _sasValidForMinutes = options.Value.SasValidForMinutes;
  }

  public async Task<Uri?> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync(cancellationToken))
    {
      _logger.LogBlobContainerDoesNotExist(_containerName);

      return null;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    try
    {
      await blobClient.UploadAsync(fileStream, cancellationToken);

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

  public async Task<Uri?> GetReadOnlySasUrlAsync(string fileName)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!await blobContainerClient.ExistsAsync())
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

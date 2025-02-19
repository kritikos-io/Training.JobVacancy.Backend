namespace Adaptit.Training.JobVacancy.Web.Server.Services;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

public class BlobStorageService
{
  private readonly BlobServiceClient _blobServiceClient;
  private readonly string _containerName;
  private readonly ILogger<BlobStorageService> _logger;

  public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
  {
    _logger = logger;
    var connectionString = Environment.GetEnvironmentVariable("AzureBlobStorage__ConnectionString");
    _blobServiceClient = new BlobServiceClient(connectionString);
    _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "resumes";
  }

  public async Task<Uri?> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!blobContainerClient.Exists())
    {
      _logger.LogWarning("Blob container {ContainerName} does not exist", _containerName);
      return null;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    try
    {
      await blobClient.UploadAsync(fileStream, cancellationToken);

      return blobClient.Uri;
    }
    catch(RequestFailedException ex)
    {
      _logger.LogWarning(ex, "Failed to upload file {FileName}", fileName);

      return null;
    }
  }

  public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!blobContainerClient.Exists())
    {
      _logger.LogWarning("Blob container {ContainerName} does not exist", _containerName);

      return false;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var isDeleted = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

    return isDeleted;
  }

  public Uri? GetReadOnlySasUrl(string fileName, int validForMinutes = 60)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

    if (!blobContainerClient.Exists())
    {
      _logger.LogWarning("Blob container {ContainerName} does not exist", _containerName);

      return null;
    }

    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var sasBuilder = new BlobSasBuilder
    {
      BlobContainerName = _containerName,
      BlobName = fileName,
      Resource = "b",
      ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(validForMinutes)
    };

    sasBuilder.SetPermissions(BlobSasPermissions.Read);

    var sasUri = blobClient.GenerateSasUri(sasBuilder);

    return sasUri;
  }
}

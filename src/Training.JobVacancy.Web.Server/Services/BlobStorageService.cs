namespace Adaptit.Training.JobVacancy.Web.Server.Services;

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
    var blobClient = blobContainerClient.GetBlobClient(fileName);

    await blobClient.UploadAsync(fileStream, cancellationToken);

    return blobClient.Uri;
  }

  public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var isDeleted = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

    return isDeleted;
  }

  public Uri GetReadOnlySasUrl(string fileName, int validForMinutes = 60)
  {
    try
    {
      var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
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
    catch(Exception ex)
    {
      _logger.LogError(ex, "Unexpected error while generating SAS URL for file {FileName}", fileName);
      throw;
    }
  }

}

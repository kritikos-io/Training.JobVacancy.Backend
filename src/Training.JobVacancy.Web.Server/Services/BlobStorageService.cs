namespace Adaptit.Training.JobVacancy.Web.Server.Services;

using Azure.Storage.Blobs;
using Azure.Storage.Sas;

public class BlobStorageService
{
  private readonly BlobServiceClient _blobServiceClient;
  private readonly string _containerName;

  public BlobStorageService(IConfiguration configuration)
  {
    var connectionString = Environment.GetEnvironmentVariable("AzureBlobStorage__ConnectionString");
    _blobServiceClient = new BlobServiceClient(connectionString);
    _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "resumes";
  }

  public async Task<string?> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    var blobClient = blobContainerClient.GetBlobClient(fileName);

    await blobClient.UploadAsync(fileStream, cancellationToken);

    return blobClient.Uri.AbsoluteUri;
  }

  public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
  {
    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var isDeleted = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

    return isDeleted;
  }


}

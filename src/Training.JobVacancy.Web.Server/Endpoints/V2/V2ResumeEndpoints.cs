namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Server.Services;

using Microsoft.AspNetCore.Http.HttpResults;

public class V2ResumeEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("resumes").WithTags("Resume");

    group.MapPost("{userId:guid}", UploadUserResume).DisableAntiforgery();
    group.MapDelete("{userId:guid}", DeleteUserResume).DisableAntiforgery();

    return endpoint;
  }


  public static async Task<Results<Ok<string>, BadRequest<string>>> UploadUserResume(
    IFormFile file,
    Guid userId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    const long fileSize = 5 * 1024 * 1024;

    if (file.Length == 0 || file.Length > fileSize || file.ContentType != "application/pdf")
    {
      logger.LogInformation("Rejected a file of type {fileType} that user tried to upload", file.ContentType);

      return TypedResults.BadRequest("Need a pdf file less than 5Mb");
    }

    var fileExtension = Path.GetExtension(file.FileName);
    var uniqueFileName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{fileExtension}";

    await using var stream = file.OpenReadStream();
    var fileUrl = await blobStorageService.UploadFileAsync(stream, uniqueFileName, cancellationToken);

    if (string.IsNullOrEmpty(fileUrl))
    {
      logger.LogWarning("File could not be uploaded");

      return TypedResults.BadRequest("Error uploading the file.");
    }

    var user = await dbContext.Users.FindAsync(userId, cancellationToken);

    if (user == null)
    {
      logger.LogWarning("User with id: {id} could not be found", userId);
    }

    if (!string.IsNullOrEmpty(user.Resume))
    {
      var fileOldUrl = new Uri(user.Resume);
      var fileName = Path.GetFileName(fileOldUrl.LocalPath);

      var deleted = await blobStorageService.DeleteFileAsync(fileName, cancellationToken);
      if (!deleted)
      {
        logger.LogWarning("Failed to delete old resume with name {fileName}", fileName );
      }
    }

    user.Resume = fileUrl;

    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok(fileUrl);
  }

  public static async Task<Results<Ok, NotFound<string>>> DeleteUserResume(
    Guid userId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    CancellationToken cancellationToken,
    ILogger<V2UserEndpoints> logger)
  {
    var user = await dbContext.Users.FindAsync(userId, cancellationToken);

    if (user == null || string.IsNullOrEmpty(user.Resume))
    {
      logger.LogInformation("User with id: {id} either not found or resume missing", userId);

      return TypedResults.NotFound("Resume not found.");
    }

    var fileUrl = new Uri(user.Resume);
    var fileName = Path.GetFileName(fileUrl.LocalPath);

    var deleted = await blobStorageService.DeleteFileAsync(fileName, cancellationToken);

    if (!deleted)
    {
      return TypedResults.NotFound("File not found or already deleted.");
    }

    user.Resume = string.Empty;
    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok();
  }


}

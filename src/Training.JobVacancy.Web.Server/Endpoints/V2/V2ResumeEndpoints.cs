namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Server.Services;

using Azure;

using Microsoft.AspNetCore.Http.HttpResults;

public class V2ResumeEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("resumes").WithTags("Resume");

    group.MapPost("{userId:guid}", UploadUserResume).DisableAntiforgery();
    group.MapDelete("{userId:guid}", DeleteUserResume).DisableAntiforgery();
    group.MapGet("{userId:guid}", GetUserResumeSasUrl).DisableAntiforgery();

    return endpoint;
  }


  public static async Task<Results<Ok<Uri>, BadRequest<string>>> UploadUserResume(
    IFormFile file,
    Guid userId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.FindAsync(userId, cancellationToken);

    if (user == null)
    {
      logger.LogWarning("User with id: {id} could not be found", userId);

      return TypedResults.BadRequest("User not found.");
    }

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

    if (fileUrl == null)
    {
      logger.LogWarning("File could not be uploaded");

      return TypedResults.BadRequest("Error uploading the file.");
    }

    if (user.Resume != null)
    {
      var fileName = Path.GetFileName(user.Resume.LocalPath);

      var deleted = await blobStorageService.DeleteFileAsync(fileName, cancellationToken);
      if (!deleted)
      {
        logger.LogWarning("Failed to delete old resume with name {fileName}", fileName);
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

    if (user == null || user.Resume == null)
    {
      logger.LogInformation("User with id: {id} either not found or resume missing", userId);

      return TypedResults.NotFound("Resume not found.");
    }

    if (!user.Resume.IsAbsoluteUri)
    {
      logger.LogWarning("User with id: {id} has an invalid resume URL", userId);

      return TypedResults.NotFound("Invalid resume URL.");
    }

    var fileName = Path.GetFileName(user.Resume.LocalPath);
    var deleted = await blobStorageService.DeleteFileAsync(fileName, cancellationToken);

    if (!deleted)
    {
      return TypedResults.NotFound("File not found or already deleted.");
    }

    user.Resume = null;
    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok();
  }

  public static async Task<Results<Ok<Uri>, NotFound<string>, ProblemHttpResult>> GetUserResumeSasUrl(
    Guid userId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    ILogger<V2ResumeEndpoints> _logger,
    CancellationToken cancellationToken)
  {
    try
    {
      var user = await dbContext.Users.FindAsync(userId, cancellationToken);

      if (user == null || user.Resume == null)
      {
        return TypedResults.NotFound("Resume not found.");
      }

      var fileName = Path.GetFileName(user.Resume.LocalPath);

      if (string.IsNullOrEmpty(fileName))
      {
        return TypedResults.Problem("Could not extract file name from the resume URL.");
      }

      var sasUrl = blobStorageService.GetReadOnlySasUrl(fileName, 60);

      return TypedResults.Ok(sasUrl);
    }
    catch (RequestFailedException ex)
    {
      _logger.LogError(ex, "Something went wrong while accessing the resume file for user {UserId}.", userId);
      return TypedResults.Problem("There was a problem accessing the resume file.");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unexpected error occurred for user {UserId}.", userId);
      return TypedResults.Problem("An unexpected error occurred. Please try again later.");
    }
  }
}

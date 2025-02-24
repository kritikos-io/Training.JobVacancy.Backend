namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Server.Services;

using Azure;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class V2ResumeEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("resumes").WithTags("Resume");

    group.MapGet(string.Empty, GetAllResumes);
    group.MapPost("{userId:guid}", UploadUserResume).DisableAntiforgery();
    group.MapDelete("{userId:guid}/{resumeId:int}", DeleteUserResume);
    group.MapGet("{userId:guid}", GetUserLatestResumeSasUrl);
    group.MapPut("{resumeId:int}", UpdateResumeUsage);

    return endpoint;
  }


  public static async Task<Results<Ok<Uri>, BadRequest<string>,NotFound<string>,InternalServerError>> UploadUserResume(
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
      logger.LogEntityNotFound(nameof(user), userId);

      return TypedResults.NotFound("User not found.");
    }

    const long fileSize = 5 * 1024 * 1024;

    if (file.Length == 0 || file.Length > fileSize || file.ContentType != "application/pdf")
    {
      logger.LogFailedToUploadFile(file.FileName);

      return TypedResults.BadRequest("Need a pdf file less than 5Mb");
    }

    var fileExtension = Path.GetExtension(file.FileName);
    var uniqueFileName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{fileExtension}";

    await using var stream = file.OpenReadStream();
    var fileUrl = await blobStorageService.UploadFileAsync(stream, uniqueFileName, cancellationToken);

    if (fileUrl == null)
    {
      logger.LogFailedToUploadFile(file.FileName);

      return TypedResults.InternalServerError();
    }

    dbContext.Resumes.Add(new Resume
    {
      DownloadUrl = fileUrl,
      User = user
    });

    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok(fileUrl);
  }

  public static async Task<Results<Ok, NotFound<string>>> DeleteUserResume(
    Guid userId,
    int resumeId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    CancellationToken cancellationToken,
    ILogger<V2UserEndpoints> logger)
  {
    var user = await dbContext.Users.Include(u => u.Resumes).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    var resumeToDelete = user.Resumes.FirstOrDefault(r => r.Id == resumeId);

    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), userId);

      return TypedResults.NotFound("Resume not found.");
    }

    if (resumeToDelete == null)
    {
      logger.LogEntityNotFound(nameof(resumeToDelete), resumeId);

      return TypedResults.NotFound("Resume not found.");
    }

    var fileName = Path.GetFileName(resumeToDelete.DownloadUrl.LocalPath);
    var deleted = await blobStorageService.DeleteFileAsync(fileName, cancellationToken);

    logger.LogDeletingEntityOfTypeWithId(nameof(resumeToDelete), resumeId);

    if (!deleted)
    {
      return TypedResults.NotFound("File not found or already deleted.");
    }

    user.Resumes.Remove(resumeToDelete);
    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok();
  }

  public static async Task<Results<Ok<Uri>, NotFound<string>, ProblemHttpResult>> GetUserLatestResumeSasUrl(
    Guid userId,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    ILogger<V2ResumeEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.Include(u => u.Resumes).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

    if (user == null || user.Resumes.Count == 0)
    {
      logger.LogEntityNotFound(nameof(user), userId);

      return TypedResults.NotFound("Resume not found.");
    }

    var latestResume = user.Resumes.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
    var fileName = Path.GetFileName(latestResume.DownloadUrl.LocalPath);

    if (string.IsNullOrEmpty(fileName))
    {
      logger.LogCouldNotExtractFileName(fileName);

      return TypedResults.Problem("Something went wrong when processing the file", statusCode: 500);
    }

    var sasUri = await blobStorageService.GetReadOnlySasUrlAsync(fileName);

    if (sasUri == null)
    {
      return TypedResults.Problem("Something went wrong while interacting with the storage service", statusCode: 500);
    }

    return TypedResults.Ok(sasUri);
  }

  public static async Task<Results<Ok, NotFound>> UpdateResumeUsage(
    int resumeId,
    JobVacancyDbContext dbContext,
    ILogger<V2ResumeEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var resumeToUpdate = await dbContext.Resumes.FindAsync(resumeId, cancellationToken);

    if (resumeToUpdate == null)
    {
      logger.LogEntityNotFound(nameof(resumeToUpdate), resumeId);

      return TypedResults.NotFound();
    }

    resumeToUpdate.IsInUse = !resumeToUpdate.IsInUse;

    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok();
  }
}

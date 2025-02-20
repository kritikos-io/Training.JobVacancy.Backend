namespace Adaptit.Training.JobVacancy.Backend.Helpers;

public static partial class LogTemplates
{
  public const string UnhandledException = "An unhandled exception occurred.";

  public const string ApiValidationException = "Validation error on {Endpoint} for {Parameter}";

  public const string EntityNotFound = "Requested entity {Entity} with id {Id} was not found.";

  public const string DeletingEntityOfTypeWithId = "Deleting entity of type {Entity} with id {Id}";

  public const string CouldNotDeleteEntityOfTypeWithId = "Could not delete entity of type {Entity} with id {Id}";

  public const string CouldNotExtractFileName = "Could not extract file name from {FileName}";

  public const string BlobContainerDoesNotExist = "Blob container {ContainerName} does not exist";

  public const string FailedToUploadFile = "Failed to upload file {FileName}";


  [LoggerMessage(LogLevel.Information, UnhandledException)]
  public static partial void LogUnhandledException(this ILogger logger);

  [LoggerMessage(LogLevel.Error, ApiValidationException)]
  public static partial void LogApiValidationException(this ILogger logger, string parameter, string endpoint);

  [LoggerMessage(LogLevel.Warning, EntityNotFound)]
  public static partial void LogEntityNotFound(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Information, DeletingEntityOfTypeWithId)]
  public static partial void LogDeletingEntityOfTypeWithId(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, CouldNotDeleteEntityOfTypeWithId)]
  public static partial void LogCouldNotDeleteEntityOfTypeWithId(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, CouldNotExtractFileName)]
  public static partial void LogCouldNotExtractFileName(this ILogger logger, string fileName);

  [LoggerMessage(LogLevel.Warning, BlobContainerDoesNotExist)]
  public static partial void LogBlobContainerDoesNotExist(this ILogger logger, string containerName);

  [LoggerMessage(LogLevel.Warning, FailedToUploadFile)]
  public static partial void LogFailedToUploadFile(this ILogger logger, string fileName);

}

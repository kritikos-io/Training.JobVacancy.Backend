namespace Adaptit.Training.JobVacancy.Web.Server.Helpers;

public static partial class LogTemplates
{
  private const string UnhandledException = "An unhandled exception occurred.";

  private const string ApiValidationException = "Validation error on {Endpoint} for {Parameter}";

  private const string EntityNotFound = "Requested entity {Entity} with id {Id} was not found.";

  private const string EntityNotCreated = "Requested entity {Entity} with name {Id} was not created.";

  private const string EntityNotUpdated = "Requested entity {Entity} with id {Id} was not created.";

  private const string EntityNotDeleted = "Requested entity {Entity} with id {Id} was not deleted.";

  public const string DeletingEntityOfTypeWithId = "Deleting entity of type {Entity} with id {Id}";

  public const string CouldNotExtractFileName = "Could not extract file name from {FileName}";

  public const string BlobContainerDoesNotExist = "Blob container {ContainerName} does not exist";

  public const string FailedToUploadFile = "Failed to upload file {FileName}";

  public const string AzureRequestFailed = "Azure request failed with message {message}";


  [LoggerMessage(LogLevel.Information, UnhandledException)]
  public static partial void LogUnhandledException(this ILogger logger);

  [LoggerMessage(LogLevel.Error, ApiValidationException)]
  public static partial void LogApiValidationException(this ILogger logger, string parameter, string endpoint);

  [LoggerMessage(LogLevel.Warning, EntityNotFound)]
  public static partial void LogEntityNotFound(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, EntityNotUpdated)]
  public static partial void LogEntityNotUpdated(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, EntityNotCreated)]
  public static partial void LogEntityNotCreated(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, EntityNotDeleted)]
  public static partial void LogEntityNotDeleted(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Information, DeletingEntityOfTypeWithId)]
  public static partial void LogDeletingEntityOfTypeWithId(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Warning, CouldNotExtractFileName)]
  public static partial void LogCouldNotExtractFileName(this ILogger logger, string fileName);

  [LoggerMessage(LogLevel.Warning, BlobContainerDoesNotExist)]
  public static partial void LogBlobContainerDoesNotExist(this ILogger logger, string containerName);

  [LoggerMessage(LogLevel.Warning, FailedToUploadFile)]
  public static partial void LogFailedToUploadFile(this ILogger logger, string fileName);

  [LoggerMessage(LogLevel.Error, AzureRequestFailed)]
  public static partial void LogAzureRequestFailed(this ILogger logger, string message);

}

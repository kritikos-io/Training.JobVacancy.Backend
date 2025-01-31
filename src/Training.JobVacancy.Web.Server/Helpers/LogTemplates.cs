namespace Adaptit.Training.JobVacancy.Backend.Helpers;

public static partial class LogTemplates
{
  public const string UnhandledException = "An unhandled exception occurred.";

  public const string ApiValidationException = "Validation error on {Endpoint} for {Parameter}";

  public const string EntityNotFound = "Requested entity {Entity} with id {Id} was not found.";

  [LoggerMessage(LogLevel.Information, UnhandledException)]
  public static partial void LogUnhandledException(this ILogger logger);

  [LoggerMessage(LogLevel.Error, ApiValidationException)]
  public static partial void LogApiValidationException(this ILogger logger, string parameter, string endpoint);

  [LoggerMessage(LogLevel.Warning, EntityNotFound)]
  public static partial void LogEntityNotFound(this ILogger logger, string entity, object id);
}

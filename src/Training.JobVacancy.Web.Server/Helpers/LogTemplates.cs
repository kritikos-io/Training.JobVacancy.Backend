namespace Adaptit.Training.JobVacancy.Backend.Helpers;

public static partial class LogTemplates
{
  public const string UnhandledException = "An unhandled exception occurred.";

  public const string ApiValidationException = "Validation error on {Endpoint} for {Parameter}";

  public const string EntityNotFound = "Requested entity {Entity} with id {Id} was not found.";

  public const string GettingEntitiesOfType = "Geting all entities of type {Entity}";

  public const string GettingEntityOfTypeWithId = "Getting entity of type {Entity} with id {Id}";

  public const string DeletingEntityOfTypeWithId = "Deleting entity of type {Entity} with id {Id}";

  public const string CreatingEntityOfType = "Creating entity of type {Entity}";

  public const string UpdatingEntityOfTypeWithId = "Updating entity of type {Entity} with id {Id}";

  [LoggerMessage(LogLevel.Information, UnhandledException)]
  public static partial void LogUnhandledException(this ILogger logger);

  [LoggerMessage(LogLevel.Error, ApiValidationException)]
  public static partial void LogApiValidationException(this ILogger logger, string parameter, string endpoint);

  [LoggerMessage(LogLevel.Warning, EntityNotFound)]
  public static partial void LogEntityNotFound(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Debug, GettingEntitiesOfType)]
  public static partial void LogGettingEntitiesOfType(this ILogger logger, string entity);

  [LoggerMessage(LogLevel.Debug, GettingEntityOfTypeWithId)]
  public static partial void LogGettingEntityOfTypeWithId(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Debug, DeletingEntityOfTypeWithId)]
  public static partial void LogDeletingEntityOfTypeWithId(this ILogger logger, string entity, object id);

  [LoggerMessage(LogLevel.Debug, CreatingEntityOfType)]
  public static partial void LogCreatingEntityOfType(this ILogger logger, string entity);

  [LoggerMessage(LogLevel.Debug, UpdatingEntityOfTypeWithId)]
  public static partial void LogUpdatingEntityOfTypeWithId(this ILogger logger, string entity, object id);
}

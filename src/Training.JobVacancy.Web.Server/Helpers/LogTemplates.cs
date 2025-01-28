namespace Adaptit.Training.JobVacancy.Backend.Helpers;

public static partial class LogTemplates
{
  public const string UnhandledException = "An unhandled exception occurred.";


  [LoggerMessage(LogLevel.Information, UnhandledException)]
  public static partial void LogUnhandledException(this ILogger logger);
}

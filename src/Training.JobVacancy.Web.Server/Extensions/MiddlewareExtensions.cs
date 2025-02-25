namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Web.Server.Middlewares;

public static class MiddlewareExtensions
{
  public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<CorrelationIdMiddleware>();
}

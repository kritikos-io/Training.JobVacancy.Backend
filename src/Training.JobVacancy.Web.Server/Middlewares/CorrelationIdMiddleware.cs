namespace Adaptit.Training.JobVacancy.Web.Server.Middlewares;

public class CorrelationIdMiddleware : IMiddleware
{
  /// <inheritdoc />
  public Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    return next.Invoke(context);
  }
}

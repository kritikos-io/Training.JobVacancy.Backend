namespace Adaptit.Training.JobVacancy.Web.Server.Middlewares;

public class CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger) : IMiddleware
{
  private readonly string header = "X-Correlation-Id";
  private readonly ILogger<CorrelationIdMiddleware> logger = logger;

  /// <inheritdoc />
  public Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    var headerValue = string.IsNullOrEmpty(context.Request.Headers[header])
      ? Guid.NewGuid().ToString()
      : (string?)context.Request.Headers[header];

    context.Request.Headers[header] = headerValue;
    context.Response.Headers[header] = headerValue;

    using (logger.BeginScope(new Dictionary<string, object> {{"Correlation-Id", headerValue}}))
    {
      return next.Invoke(context);
    }
  }
}

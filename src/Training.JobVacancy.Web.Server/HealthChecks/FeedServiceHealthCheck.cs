namespace Adaptit.Training.JobVacancy.Web.Server.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class FeedServiceHealthCheck : IHealthCheck
{
  private readonly HttpClient httpClient = new() {Timeout = TimeSpan.FromSeconds(2)};

  /// <inheritdoc />
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
    CancellationToken cancellationToken = new())
  {
    try
    {
      var reply = await httpClient.GetAsync("http://localhost:8080/api/v1/feed", cancellationToken);
      return !reply.IsSuccessStatusCode
        ? HealthCheckResult.Unhealthy("Feed service unreachable.")
        : HealthCheckResult.Healthy();
    }
    catch (Exception)
    {
      return HealthCheckResult.Unhealthy("Feed service unreachable.");
    }
  }
}

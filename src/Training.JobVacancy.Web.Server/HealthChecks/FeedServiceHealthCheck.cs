namespace Adaptit.Training.JobVacancy.Web.Server.HealthChecks;

using Adaptit.Training.JobVacancy.Web.Models;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class FeedServiceHealthCheck(INavJobVacancy navJobVacancy) : IHealthCheck
{
  private readonly INavJobVacancy navJobVacancy = navJobVacancy;

  /// <inheritdoc />
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
    CancellationToken cancellationToken = new())
  {
    try
    {
      var responce = await navJobVacancy.GetLatestFeedPageAsync(cancellationToken: cancellationToken);
      if (!responce.IsSuccessful)
      {
        HealthCheckResult.Unhealthy("Feed service unreachable.");
      }
      return HealthCheckResult.Healthy();
    }
    catch (HttpRequestException)
    {
      return HealthCheckResult.Unhealthy("Network error when contacting Feed API");
    }
  }
}

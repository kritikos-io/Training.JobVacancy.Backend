namespace Adaptit.Training.JobVacancy.Web.Server.HealthChecks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class DatabaseHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck
  where TDbContext : DbContext
{
  private readonly TDbContext dbContext = dbContext;

  /// <inheritdoc />
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
    CancellationToken cancellationToken = new CancellationToken())
  {
    try
    {
      if (!await dbContext.Database.CanConnectAsync(cancellationToken))
      {
        return HealthCheckResult.Unhealthy("Cannot connect to the database.");
      }

      var migrations = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
      return migrations.Count != 0
        ? HealthCheckResult.Degraded($"There are {migrations.Count} pending migrations.")
        : HealthCheckResult.Healthy();
    }
    catch
    {
      return HealthCheckResult.Unhealthy("Database not accessible !");
    }
  }
}

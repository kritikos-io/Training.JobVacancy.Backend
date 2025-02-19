namespace Adaptit.Training.JobVacancy.Web.Server.HealthChecks;

using Adaptit.Training.JobVacancy.Web.Server.Options;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

public class OidcHealthCheck(IOptions<JobVacancyAuthenticationOptions> options, IHttpClientFactory clientFactory) : IHealthCheck
{
  private readonly JobVacancyAuthenticationOptions options = options.Value;

  private readonly IHttpClientFactory clientFactory = clientFactory;

  /// <inheritdoc />
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
  {
    var client = clientFactory.CreateClient();
    string? response;
    try
    {
      response = await client.GetStringAsync($"{options.Authority}/.well-known/openid-configuration", cancellationToken);
    }
    catch (HttpRequestException)
    {
      return HealthCheckResult.Unhealthy("OpenID Connect Provider unreachable.");
    }

    try
    {
      OpenIdConnectConfiguration.Create(response);
    }
    catch (ArgumentException)
    {
      return HealthCheckResult.Unhealthy("OpenID Connect Provider configuration invalid.");
    }

    return HealthCheckResult.Healthy();
  }
}

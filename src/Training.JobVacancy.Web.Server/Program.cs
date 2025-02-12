using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Server;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.HealthChecks;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddJobVacancyServices();

builder.AddMiddlewareServices();

builder.Services.AddHealthChecks()
  .AddCheck<DatabaseHealthCheck<JobVacancyDbContext>>("Database", tags: ["ready"])
  .AddCheck<OidcHealthCheck>("OpenID Connect Provider", tags: ["ready"])
  .AddCheck<FeedServiceHealthCheck>("Feed Service", tags: ["ready"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});
app.MapHealthChecks("health/ready", new HealthCheckOptions
{
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
  Predicate = health => health.Tags.Contains("ready"),
});

app.MapHealthChecks("health/live", new HealthCheckOptions
{
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
  Predicate = health => health.Tags.Contains("live"),
});

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
  options
      .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
      .WithPreferredScheme("openid")
      .WithOAuth2Authentication(oauth =>
      {
        oauth.ClientId = "Swagger";
        oauth.Scopes = new[] { "openid", "profile", "email" };
      })
      .WithLayout(ScalarLayout.Modern);
});

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEndpoints();

app.UseCorrelationIdMiddleware();

app.Run();

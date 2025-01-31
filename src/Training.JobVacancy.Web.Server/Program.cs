using Adaptit.Training.JobVacancy.Web.Server;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddJobVacancyServices();

builder.AddMiddlewareServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEndpoints();

app.UseCorrelationIdMiddleware();

app.MapGet("/api/v2/hello", (HttpContext ctx, ILogger<Program> logger) =>
{
  logger.LogWarning("Received Requeset");
  return ctx.Request.Headers;
});

app.Run();

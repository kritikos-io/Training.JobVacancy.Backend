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

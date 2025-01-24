using Adaptit.Training.JobVacancy.Backend.Endpoints;
using Adaptit.Training.JobVacancy.Data;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobDatabase>(options =>
{
  options.UseSqlite(builder.Configuration.GetConnectionString("JobDatabase"),
          options => options
              .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
      .EnableSensitiveDataLogging()
      .EnableDetailedErrors();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapWeatherEndpoints();

app.Run();

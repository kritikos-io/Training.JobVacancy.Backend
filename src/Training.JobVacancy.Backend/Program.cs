using Adaptit.Training.JobVacancy.Backend.Endpoints;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<OpenIdConnectOptions>();
builder.Services.AddTransient<OpenIdConnectOptions>();
builder.Services.AddScoped<OpenIdConnectOptions>();

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

using Adaptit.Training.JobVacancy.Backend.Endpoints;
using Adaptit.Training.JobVacancy.WebModels;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;

using Refit;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<OpenIdConnectOptions>();
builder.Services.AddRefitClient<IFeedApi>()
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri("https://pam-stilling-feed.nav.no");
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
app.MapFeedEndpoint();
app.MapFeedEntryEndpoint();

app.Run();

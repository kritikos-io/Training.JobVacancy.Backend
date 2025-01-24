namespace Adaptit.Training.JobVacancy.Backend.Endpoints;

using Adaptit.Training.JobVacancy.Backend.Dto;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class WeatherForecastEndpoints
{
  static string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

  public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
  {

    var api = endpoints.MapGroup("/api/v2/weatherforecast")
        .WithOpenApi();

    api.MapGet("", GetAllForecasts);
    api.MapGet("/{id:int}", GetForecast);

    return endpoints;
  }

  public static async Task<List<WeatherForecast>> GetAllForecasts([FromHeader]string ifNotModified)
  {
    var forecast = Enumerable.Range(1, 5)
        .Select(index =>
            new WeatherForecast(
                0,
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToList();

    return forecast;
  }

  public static async Task<Results<Ok<WeatherForecast>, BadRequest, NotFound<string>>> GetForecast(int id)
  {
    var y = (WeatherForecast x) => x.Date;

    var forecast = new WeatherForecast(
        id,
        DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    );

    return TypedResults.Ok(forecast);
  }
}

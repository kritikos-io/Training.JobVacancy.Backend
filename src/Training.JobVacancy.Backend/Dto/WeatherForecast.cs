namespace Adaptit.Training.JobVacancy.Backend.Dto;

public record WeatherForecast(int Id, DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

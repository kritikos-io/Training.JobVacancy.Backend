namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class EmployerDto
{
  public string Name { get; set; } = string.Empty;

  [JsonPropertyName("orgnr")]
  public string Organizationnumber { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public Uri HomePage { get; set; } = new Uri("about:blank");
}

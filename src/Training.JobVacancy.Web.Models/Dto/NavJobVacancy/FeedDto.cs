namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class FeedDto
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public string Version { get; set; } = string.Empty;

  [JsonPropertyName("next_id")]
  public Guid? NextId { get; set; }

  [JsonPropertyName("home_page_url")]
  public Uri HomePageUrl { get; set; } = new Uri("about:blank");

  public VacancyDto[] Items { get; set; } = [];
}

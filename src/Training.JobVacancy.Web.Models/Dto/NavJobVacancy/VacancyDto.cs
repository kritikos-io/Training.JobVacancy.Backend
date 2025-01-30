namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class VacancyDto
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;

  [JsonPropertyName("content_text")]
  public string Content { get; set; } = string.Empty;

  [JsonPropertyName("date_modified")]
  public DateTimeOffset ModifiedAt { get; set; }

  [JsonPropertyName("_feed_entry")]
  public EntryDetailsDto EntryDetails { get; set; } = null!;
}

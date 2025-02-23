namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

using System.Text.Json.Serialization;

using Adaptit.Training.JobVacancy.Web.Models.Enum;

public class JobAdDto
{
  public Guid Id { get; set; }

  [JsonPropertyName("type")]
  public JobType Type { get; set; }

  [JsonPropertyName("salary_range")]
  public string SalaryRange { get; set; }

  [JsonPropertyName("description")]
  public string Description { get; set; }

  [JsonPropertyName("favorite")]
  public bool Favorite { get; set; }

  [JsonPropertyName("location")]
  public string Location { get; set; }

  [JsonPropertyName("created_at")]
  public DateTime CreatedAt { get; set; }

  [JsonPropertyName("expires_at")]
  public DateTime ExpiresAt { get; set; }

  [JsonPropertyName("level")]
  public JobExperienceLevel Level { get; set; }
}

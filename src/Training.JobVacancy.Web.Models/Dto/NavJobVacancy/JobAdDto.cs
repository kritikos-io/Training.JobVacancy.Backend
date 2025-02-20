namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

using Adaptit.Training.JobVacancy.Web.Models.Enum;

public class JobAdDto
{
  public Guid Uuid { get; set; }
  public JobType Type { get; set; }

  [JsonPropertyName("salary_range")]
  public string SalaryRange { get; set; }

  [JsonPropertyName("jobtitle")]
  public string Title { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  [JsonPropertyName("published")]
  public DateTimeOffset PublishedAt { get; set; }

  [JsonPropertyName("expires")]
  public DateTimeOffset ExpiresAt { get; set; }

  [JsonPropertyName("updated")]
  public DateTimeOffset UpdatedAt { get; set; }

  public JobExperienceLevel Level { get; set; }

  public Source Source { get; set; }

  public Uri? ApplicationUrl { get; set; }

  public DateTimeOffset ApplicationDue { get; set; }
  public EmployerDto Employer { get; set; } = null!;

  [JsonPropertyName("workLocations")]
  public WorkLocationDto[] Locations { get; set; } = [];

  [JsonPropertyName("contactList")]
  public ContactDto[] Contacts { get; set; } = [];
}

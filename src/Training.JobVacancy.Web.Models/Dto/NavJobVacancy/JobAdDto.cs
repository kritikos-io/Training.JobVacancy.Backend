namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class JobAdDto
{
  public Guid Uuid { get; set; }

  [JsonPropertyName("jobtitle")]
  public string Title { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  [JsonPropertyName("published")]
  public DateTimeOffset PublishedAt { get; set; }

  [JsonPropertyName("expires")]
  public DateTimeOffset ExpiresAt { get; set; }

  [JsonPropertyName("updated")]
  public DateTimeOffset UpdatedAt { get; set; }

  public Source Source { get; set; }

  public Uri? ApplicationUrl { get; set; }

  public DateTimeOffset ApplicationDue { get; set; }

  public Uri Link { get; set; } = new Uri("about:blank");

  public string Extent { get; set; } = string.Empty;

  public string PositionCount { get; set; } = string.Empty;

  public string Sector { get; set; }

  public string EngagementType { get; set; } = string.Empty;

  public EmployerDto Employer { get; set; } = null!;

  [JsonPropertyName("categoryList")]
  public CategoryDto[] Categories { get; set; } = [];

  [JsonPropertyName("workLocations")]
  public WorkLocationDto[] Locations { get; set; } = [];

  [JsonPropertyName("contactList")]
  public ContactDto[] Contacts { get; set; } = [];

  public string starttime { get; set; } = string.Empty;
  public string sourceurl { get; set; } = string.Empty;
  public OccupationCategories[] occupationCategories { get; set; } = [];
}

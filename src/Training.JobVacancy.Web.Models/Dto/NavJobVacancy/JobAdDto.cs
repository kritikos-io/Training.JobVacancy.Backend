namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class JobAdDto
{
  public Guid Uuid { get; set; }

  [JsonPropertyName("jobtitle")]
  public string Title { get; set; }

  public string Description { get; set; }

  [JsonPropertyName("published")]
  public DateTimeOffset PublishedAt { get; set; }

  [JsonPropertyName("expires")]
  public DateTimeOffset ExpiresAt { get; set; }

  [JsonPropertyName("updated")]
  public DateTimeOffset UpdatedAt { get; set; }

  [JsonPropertyName("workLocations")]
  public WorkLocationDto[] Locations { get; set; }

  [JsonPropertyName("contactList")]
  public ContactDto[] Contacts { get; set; }


  public Source source { get; set; }
  public Uri? ApplicationUrl { get; set; }

  public DateTimeOffset ApplicationDue { get; set; }

  [JsonPropertyName("categoryList")]
  public CategoryDto[] Categories { get; set; }

  public Uri Link { get; set; }

  public EmployerDto Employer { get; set; }
  public string Extent { get; set; }

  public string PositionCount { get; set; }

  public string Sector { get; set; }

  public string EngagementType { get; set; }
  public OccupationCategories[] occupationCategories { get; set; }
  public string starttime { get; set; }
  public string sourceurl { get; set; }
}

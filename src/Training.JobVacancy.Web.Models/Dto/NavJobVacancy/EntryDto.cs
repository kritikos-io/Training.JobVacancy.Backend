namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class EntryDto
{
  public Guid Uuid { get; set; }

  [JsonPropertyName("ad_content")]
  public JobAdDto? JobAd { get; set; }

  public DateTimeOffset ModifiedAt { get; set; }

  public EntryStatus Status { get; set; }
}

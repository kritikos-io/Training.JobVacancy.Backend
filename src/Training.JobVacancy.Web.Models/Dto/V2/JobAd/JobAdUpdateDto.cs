namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

using Adaptit.Training.JobVacancy.Web.Models.Enum;

public class JobAdUpdateDto
{
  public JobType Type { get; set; }

  public string SalaryRange { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public string Location { get; set; } = string.Empty;

  public DateTime ExpiresAt { get; set; }

  public JobExperienceLevel Level { get; set; }
}

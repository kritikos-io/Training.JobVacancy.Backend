namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

using Adaptit.Training.JobVacancy.Web.Models.Enum;

public class JobAdCreateDto
{
  public Guid CompanyId { get; set; }
  public string Description { get; set; }

  public string Location { get; set; }

  public string SalaryRange { get; set; }

  public DateTime ExpiresAt { get; set; }

  public JobType Type { get; set; }

  public JobExperienceLevel Level { get; set; }
}

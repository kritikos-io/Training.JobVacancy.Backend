namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Models.Enum;

public class JobAdResponseDto
{
  public Guid Id { get; set; }

  public JobType Type { get; set; }

  public string SalaryRange { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public bool Favorite { get; set; }

  public string Location { get; set; } = string.Empty;

  public DateTime CreatedAt { get; set; }

  public DateTime ExpiresAt { get; set; }

  public JobExperienceLevel Level { get; set; }

  public bool IsFavorite { get; set; }

  public Guid CompanyId { get; set; }
}

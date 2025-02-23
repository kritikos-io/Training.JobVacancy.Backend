namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;
using Adaptit.Training.JobVacancy.Web.Models.Enum;

public static class EntityDtoTransformations
{
  public static JobAd ToEntity(this JobAdDto dto) => new()
  {
    Id = dto.Id,
    Type = dto.Type,
    SalaryRange = dto.SalaryRange,
    Description = dto.Description,
    Favorite = dto.Favorite,
    Location = dto.Location,
    CreatedAt = dto.CreatedAt,
    ExpiresAt = dto.ExpiresAt,
    Level = dto.Level,
  };

  public static JobAdDto ToDto(this JobAd jobAd) => new()
  {
    Id = Guid.NewGuid(),
    Type = jobAd.Type,
    SalaryRange = jobAd.SalaryRange,
    Description = jobAd.Description,
    Favorite = jobAd.Favorite,
    Location = jobAd.Location,
    CreatedAt = jobAd.CreatedAt,
    ExpiresAt = jobAd.ExpiresAt,
    Level = jobAd.Level,
  };

  public static void UpdateEntity(this JobAd jobAd, JobAdUpdateDto dto)
  {
    jobAd.Type = (JobType)dto.Type!;
    jobAd.SalaryRange = dto.SalaryRange;
    jobAd.Description = dto.Description;
    jobAd.Favorite = dto.Favorite;
    jobAd.Location = dto.Location;
    jobAd.CreatedAt = (DateTime)dto.CreatedAt!;
    jobAd.ExpiresAt = (DateTime)dto.ExpiresAt!;
    jobAd.Level = (JobExperienceLevel)dto.Level!;
  }
}

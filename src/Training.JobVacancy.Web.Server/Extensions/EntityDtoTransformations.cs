namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;

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
    jobAd.Type = dto.Type ?? jobAd.Type;
    jobAd.SalaryRange = dto.SalaryRange ?? jobAd.SalaryRange;
    jobAd.Description = dto.Description ?? jobAd.Description;
    jobAd.Favorite = dto.Favorite ?? jobAd.Favorite;
    jobAd.Location = dto.Location ?? jobAd.Location;
    jobAd.CreatedAt = dto.CreatedAt ?? jobAd.CreatedAt;
    jobAd.ExpiresAt = dto.ExpiresAt ?? jobAd.ExpiresAt;
    jobAd.Level = dto.Level ?? jobAd.Level;
  }
}

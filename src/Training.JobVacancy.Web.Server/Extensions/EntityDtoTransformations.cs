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
    Location = jobAd.Location,
    CreatedAt = jobAd.CreatedAt,
    ExpiresAt = jobAd.ExpiresAt,
    Level = jobAd.Level,
  };
}

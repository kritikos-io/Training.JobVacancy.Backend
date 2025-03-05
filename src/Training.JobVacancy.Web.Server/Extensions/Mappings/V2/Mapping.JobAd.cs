﻿namespace Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

public static partial class Mapping
{
  public static JobAd ToEntity(this JobAdCreateDto dto, Company company) => new()
  {
    Id = Guid.NewGuid(),
    Type = dto.Type,
    SalaryRange = dto.SalaryRange,
    Description = dto.Description,
    Location = dto.Location,
    CreatedAt = DateTime.UtcNow,
    ExpiresAt = dto.ExpiresAt,
    Level = dto.Level,
    Company = company
  };


  public static JobAdResponseDto ToResponseDto(this JobAd jobAd, bool isFavorite) => new()
  {
    Id = Guid.NewGuid(),
    Type = jobAd.Type,
    SalaryRange = jobAd.SalaryRange,
    Description = jobAd.Description,
    Location = jobAd.Location,
    CreatedAt = jobAd.CreatedAt,
    ExpiresAt = jobAd.ExpiresAt,
    Favorite = isFavorite,
    Level = jobAd.Level,
    //Company = jobAd.Company.ToResponseDto(),
  };

  public static JobAddShortResponseDto ToShortResponseDto(this JobAd jobAd, bool isFavorite) => new()
  {
    Id = jobAd.Id,
    Type = jobAd.Type,
    SalaryRange = jobAd.SalaryRange,
    Location = jobAd.Location,
    ExpiresAt = jobAd.ExpiresAt,
    Favorite = isFavorite,
    Level = jobAd.Level,
    CompanyId = jobAd.Company.Id
  };

  public static void UpdateEntity(this JobAd jobAd, JobAdUpdateDto dto)
  {
    jobAd.Type = dto.Type;
    jobAd.SalaryRange = dto.SalaryRange;
    jobAd.Description = dto.Description;
    jobAd.Location = dto.Location;
    jobAd.ExpiresAt = dto.ExpiresAt;
    jobAd.Level = dto.Level;
  }
}

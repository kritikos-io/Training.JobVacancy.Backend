namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class V2JobAdEndpoints
{
  private const string GetJobAddEndpointName = "V2GetJobAd";

  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("jobad")
          .WithTags("JobAd");

    group.MapGet("", GetAllJobAds);
    group.MapGet("{id:guid}", GetJobAd).WithName(GetJobAddEndpointName);

    group.MapPost("", CreateJobAd);
    group.MapPatch("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);

    return endpoint;
  }

  private static async Task<Ok<PageList<JobAd>>> GetAllJobAds(
    [FromQuery] JobType? type,
    [FromQuery] bool? favorite,
    [FromQuery] double? salary,       // Not implemented
    [FromQuery] DateTime? created,
    [FromQuery] DateTime? expires,
    [FromQuery] string? description,  // Not implemented
    [FromQuery] int? page,
    [FromQuery] int? size,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobs = db.JobAds
      .WhereIf(type != null, j => j.Type == type)
      .WhereIf(favorite != null, j => j.Favorite == favorite)
      .WhereIf(created != null, j => j.CreatedAt >= created)
      .WhereIf(expires != null, j => j.ExpiresAt <= expires)
      .OrderBy(x => x.Id)
      .Page(page ?? 1, size ?? 5);

    return TypedResults.Ok(jobs);
  }

  // public static async Task SearchJobAds(Criteria c)
  // {
  //
  // }

  private static async Task<Results<Ok<JobAdDto>, NotFound>> GetJobAd(
    [FromRoute] Guid id,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobAd = await db.JobAds.FindAsync(id, cancellationToken);

    if (jobAd == null)
    {
      logger.LogEntityNotFound(nameof(JobAd), id);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(EntityToDto(jobAd));
  }

  private static async Task<Results<CreatedAtRoute<JobAd>, BadRequest<string>>> CreateJobAd(
    [FromBody] JobAdDto ad,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobAd = DtoToEntity(ad);

    db.JobAds.Add(jobAd);
    await db.SaveChangesAsync(cancellationToken);

    return TypedResults.CreatedAtRoute(jobAd, GetJobAddEndpointName);
  }

  private static Results<Ok<string>, NotFound<string>> UpdateJobAd(Guid id) => throw new NotImplementedException();

  private static async Task<Results<Ok, NotFound>> DeleteJobAd(
    [FromRoute] Guid id,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger)
  {
    var jobAd = await db.JobAds.FindAsync(id);

    if (jobAd != null)
    {
      db.JobAds.Remove(jobAd);
      await db.SaveChangesAsync(CancellationToken.None);
      return TypedResults.Ok();
    }

    logger.LogEntityNotFound(nameof(JobAd), id);
    return TypedResults.NotFound();
  }

  private static JobAd DtoToEntity(JobAdDto dto)
  {
    return new JobAd()
    {
      Id = Guid.NewGuid(),
      Type = dto.Type,
      SalaryRange = dto.SalaryRange,
      Description = dto.Description,
      Location = dto.Location,
      CreatedAt = dto.CreatedAt,
      ExpiresAt = dto.ExpiresAt,
      Level = dto.Level,
    };
  }

  private static JobAdDto EntityToDto(JobAd jobAd)
  {
    return new JobAdDto()
    {
      Id = jobAd.Id,
      Type = jobAd.Type,
      SalaryRange = jobAd.SalaryRange,
      Description = jobAd.Description,
      Location = jobAd.Location,
      CreatedAt = jobAd.CreatedAt,
      ExpiresAt = jobAd.ExpiresAt,
      Level = jobAd.Level,
    };
  }

}

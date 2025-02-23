namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.Helpers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using LogTemplates = Adaptit.Training.JobVacancy.Web.Server.Helpers.LogTemplates;

public class V2JobAdEndpoints
{
  private const string GetJobAddEndpointName = "GetJobAd";

  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("jobad")
          .WithTags("JobAd");

    group.MapGet("", ListAllJobAds);
    group.MapGet("{id:guid}", GetJobAd)
         .WithName(GetJobAddEndpointName);

    group.MapPost("/search", SearchJobAds);
    group.MapPost("", CreateJobAd);
    group.MapPut("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);

    return endpoint;
  }

  private static async Task<Ok<PagedList<JobAd>>> ListAllJobAds(
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken,
    [FromQuery] int page = 1,
    [FromQuery] int size = 20)
  {
    var jobs = await db.JobAds
      .OrderBy(x => x.Id)
      .ToPagedListAsync(page, size,cancellationToken);

    return TypedResults.Ok(jobs);
  }

  private static async Task<Results<Ok<PagedList<JobAd>>, BadRequest>> SearchJobAds(
    JobAdFilters filters,
    JobVacancyDbContext db,
    CancellationToken cancellationToken,
    [FromQuery] int page = 1,
    [FromQuery] int size = 20)
  {
    var jobAds = await db.JobAds
      .WhereIf(filters.Type != null, j => j.Type == filters.Type)
      .WhereIf(filters.Favorite != null, j => j.Favorite == filters.Favorite)
      .WhereIf(filters.Created != null, j => j.CreatedAt >= filters.Created)
      .WhereIf(filters.Expires != null, j => j.ExpiresAt <= filters.Expires)
      .WhereIf(filters.Description != null, j => EF.Functions.ToTsVector("english", j.Description).Matches(filters.Description!))
      .OrderBy(x => x.Id)
      .ToPagedListAsync(page, size, cancellationToken);

    return TypedResults.Ok(jobAds);
  }

  private static async Task<Results<Ok<JobAdDto>, NotFound>> GetJobAd(
    [FromRoute] Guid id,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobAd = await db.JobAds.FindAsync([id], cancellationToken: cancellationToken);

    if (jobAd != null)
      return TypedResults.Ok(jobAd.ToDto());

    LogTemplates.LogEntityNotFound(logger, nameof(JobAd), id);
    return TypedResults.NotFound();
  }

  private static async Task<Results<CreatedAtRoute<JobAd>, BadRequest<string>>> CreateJobAd(
    [FromBody] JobAdDto ad,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobAd = ad.ToEntity();

    db.JobAds.Add(jobAd);
    await db.SaveChangesAsync(cancellationToken);

    return TypedResults.CreatedAtRoute(jobAd, nameof(GetJobAd), new { id = jobAd.Id });
  }

  private static async Task<Results<Ok<JobAdDto>, NotFound>> UpdateJobAd(
    Guid id,
    JobAdUpdateDto dto,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger)
  {
    var jobAd = await db.JobAds.FindAsync([id], CancellationToken.None);

    if (jobAd == null)
    {
      LogTemplates.LogEntityNotFound(logger, nameof(JobAd), id);
      return TypedResults.NotFound();
    }

    jobAd.UpdateEntity(dto);
    await db.SaveChangesAsync();

    return TypedResults.Ok(jobAd.ToDto());
  }

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

    LogTemplates.LogEntityNotFound(logger, nameof(JobAd), id);
    return TypedResults.NotFound();
  }

}

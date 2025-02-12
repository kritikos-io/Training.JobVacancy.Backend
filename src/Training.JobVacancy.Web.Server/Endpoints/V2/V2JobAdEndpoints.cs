namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    group.MapPatch("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);

    return endpoint;
  }

  private static async Task<Ok<PageList<JobAd>>> ListAllJobAds(
    [FromQuery] int? page,
    [FromQuery] int? size,
    JobVacancyDbContext db,
    ILogger<V2JobAdEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var jobs = await db.JobAds
      .OrderBy(x => x.Id)
      .Page(cancellationToken, page ?? 1, size ?? 20);

    return TypedResults.Ok(jobs);
  }

  private static async Task<Results<Ok<PageList<JobAd>>, BadRequest>> SearchJobAds(
    JobAdFilters filters,
    [FromQuery] int? page,
    [FromQuery] int? size,
    JobVacancyDbContext db,
    CancellationToken cancellationToken)
  {
    var jobAds = await db.JobAds
      .WhereIf(filters.Type != null, j => j.Type == filters.Type)
      .WhereIf(filters.Favorite != null, j => j.Favorite == filters.Favorite)
      .WhereIf(filters.Created != null, j => j.CreatedAt >= filters.Created)
      .WhereIf(filters.Expires != null, j => j.ExpiresAt <= filters.Expires)
      .WhereIf(filters.Description != null, j => EF.Functions.ToTsVector("english", j.Description).Matches(filters.Description!))
      .OrderBy(x => x.Id)
      .Page(cancellationToken, page ?? 1, size ?? 20);

    return TypedResults.Ok(jobAds);
  }

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

    return TypedResults.Ok(jobAd.ToDto());
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

    return TypedResults.CreatedAtRoute(jobAd, nameof(GetJobAd), new {id = jobAd.Id});
  }

  private static Results<Ok<string>, NotFound<string>> UpdateJobAd(Guid id)
  {
    throw new NotImplementedException();
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

    logger.LogEntityNotFound(nameof(JobAd), id);
    return TypedResults.NotFound();
  }

}

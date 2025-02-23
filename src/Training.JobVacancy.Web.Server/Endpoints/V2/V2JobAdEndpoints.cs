namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using System.ComponentModel.DataAnnotations;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;
using Adaptit.Training.JobVacancy.Web.Server.Helpers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class V2JobAdEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("jobad")
        .WithTags("JobAd");

    group.MapGet("", ListAllJobAds);
    group.MapGet("{id:guid}", GetJobAd)
        .WithName(nameof(GetJobAd));

    group.MapPost("search", SearchJobAds);
    group.MapPost("", CreateJobAd);
    group.MapPut("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);
    group.MapPut("{id:guid}/favorite", FavoriteJobAd);

    return endpoint;
  }

  private static async Task<Ok<PagedList<JobAdDto>>> ListAllJobAds(
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken,
      [FromQuery][Range(1, int.MaxValue)] int page = 1,
      [FromQuery][Range(1, 50)] int size = 20)
  {
    var jobs = await db.JobAds
        .OrderBy(x => x.Id)
        .ToPagedListAsync(x => x.ToDto(), page, size, cancellationToken);

    return TypedResults.Ok(jobs);
  }

  private static async Task<Results<Ok<PagedList<JobAddShortResponseDto>>, BadRequest>> SearchJobAds(
      JobAdFilters filters,
      JobVacancyDbContext db,
      CancellationToken cancellationToken,
      [FromQuery] int page = 1,
      [FromQuery] int size = 20)
  {
    var jobAds = await db.JobAds
        .WhereIf(filters.Type != null, j => j.Type == filters.Type)
        .WhereIf(filters.Created != null, j => j.CreatedAt >= filters.Created)
        .WhereIf(filters.Expires != null, j => j.ExpiresAt <= filters.Expires)
        .WhereIf(filters.Description != null, j => EF.Functions.ToTsVector("english", j.Description).Matches(filters.Description!))
        .OrderBy(x => x.Id)
        .ToPagedListAsync(x=>x.ToShortResponseDto(), page, size, cancellationToken);

    return TypedResults.Ok(jobAds);
  }

  private static async Task<Results<Ok<JobAdDto>, NotFound>> GetJobAd(
      [FromRoute] Guid id,
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken)
  {
    var jobAd = await db.JobAds
        .Include(x => x.Company)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (jobAd is null)
    {
      logger.LogEntityNotFound(nameof(jobAd), id);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(jobAd.ToDto());
  }

  private static async Task<Results<CreatedAtRoute<JobAdDto>, ValidationProblem>> CreateJobAd(
      [FromBody] JobAdCreateDto ad,
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken)
  {
    var company = await db.Companies.FirstOrDefaultAsync(x => x.Id == ad.CompanyId, cancellationToken);
    if (company is null)
    {
      logger.LogEntityNotFound(nameof(Company), ad.CompanyId);
      return TypedResults.ValidationProblem([new KeyValuePair<string, string[]>(nameof(JobAdCreateDto.CompanyId), ["No company with that id exists."])]);
    }

    var jobAd = ad.ToEntity(company);

    db.JobAds.Add(jobAd);
    await db.SaveChangesAsync(cancellationToken);

    return TypedResults.CreatedAtRoute(jobAd.ToDto(), nameof(GetJobAd), new { id = jobAd.Id });
  }

  private static async Task<Results<Ok<JobAdDto>, NotFound>> UpdateJobAd(
      Guid id,
      JobAdUpdateDto dto,
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger)
  {
    var jobAd = await db.JobAds.FindAsync([id], CancellationToken.None);

    if (jobAd is null)
    {
      logger.LogEntityNotFound(nameof(JobAd), id);
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

    if (jobAd is null)
    {
      logger.LogEntityNotFound(nameof(JobAd), id);
      return TypedResults.NotFound();
    }

    db.JobAds.Remove(jobAd);
    await db.SaveChangesAsync(CancellationToken.None);
    return TypedResults.Ok();
  }

  private static async Task<Results<NoContent, NotFound, UnprocessableEntity>> FavoriteJobAd(
      Guid id,
      JobVacancyDbContext ctx,
      [FromServices] HttpContextAccessor accessor,
      bool favorite = true,
      CancellationToken cancellationToken = default)
  {
    var jobAd = await ctx.JobAds.FindAsync(id, cancellationToken);
    if (jobAd is null)
    {
      return TypedResults.NotFound();
    }

    var userId = accessor.HttpContext?.User.Claims.GetUserIdFromClaims();
    if (userId is null)
    {
      return TypedResults.UnprocessableEntity();
    }

    var user = await ctx.Users.FindAsync(userId, cancellationToken);
    if (user is null)
    {
      user = accessor.HttpContext!.User.Claims.ToList().MapToUser();
      ctx.Users.Add(user);
      await ctx.SaveChangesAsync(cancellationToken);
    }

    var userFavorite = await ctx.UserFavoriteJobAd
        .FirstOrDefaultAsync(x => x.JobAd.Id == id && x.User.Id == userId, cancellationToken);
    if (userFavorite is null)
    {
      userFavorite = new UserJobAd() { User = user, JobAd = jobAd, IsFavorite = favorite };
      ctx.UserFavoriteJobAd.Add(userFavorite);
      await ctx.SaveChangesAsync(cancellationToken);
    }

    return TypedResults.NoContent();
  }
}

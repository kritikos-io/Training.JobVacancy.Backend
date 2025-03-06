namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

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

    group.MapGet("{id:guid}", GetJobAd)
        .WithName(nameof(GetJobAd));

    group.MapPost("search", SearchJobAds);
    group.MapPost("", CreateJobAd);
    group.MapPut("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);
    group.MapPut("{id:guid}/favorite", FavoriteJobAd);

    return endpoint;
  }

  private static async Task<Ok<PagedList<JobAddShortResponseDto>>> SearchJobAds(
      JobAdFilters? filters,
      IHttpContextAccessor accessor,
      JobVacancyDbContext db,
      CancellationToken cancellationToken = default,
      [FromQuery] int page = 1,
      [FromQuery] int size = 20)
  {

    var userId = accessor.HttpContext?.User.Claims.GetUserIdFromClaims();

    var jobAds = await db.JobAds
      .Include(x => x.Company)
        .WhereIf(filters?.Type != null, j => j.Type == filters!.Type)
        .WhereIf(filters?.Created != null, j => j.CreatedAt >= filters!.Created)
        .WhereIf(filters?.Expires != null, j => j.ExpiresAt <= filters!.Expires)
        .WhereIf(filters?.Description != null, j => EF.Functions.ToTsVector("english", j.Description).Matches(filters!.Description!))
        .Select(j => new
        {
          JobAd = j,
          IsFavorite = userId != null && db.UserFavoriteJobAd.Any(f => f.JobAd.Id == j.Id && f.User.Id == userId)
        })
        .WhereIf(filters?.Favorite != null && userId !=null, j => j.IsFavorite == filters!.Favorite!.Value)
        .OrderBy(x => x.JobAd.Id)
        .ToPagedListAsync(x => x.JobAd.ToShortResponseDto(x.IsFavorite), page, size, cancellationToken);

    return TypedResults.Ok(jobAds);
  }

  private static async Task<Results<Ok<JobAdResponseDto>, NotFound>> GetJobAd(
      [FromRoute] Guid id,
      JobVacancyDbContext db,
      IHttpContextAccessor accessor,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken = default)
  {

    var jobAdAndUser = await GetUserJobAdMapped(id, accessor, db, cancellationToken);

    if (jobAdAndUser.JobAd is null)
    {
      logger.LogEntityNotFound(nameof(jobAdAndUser), id);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(jobAdAndUser.ToResponseDto());
  }

  private static async Task<Results<CreatedAtRoute<JobAdResponseDto>, ValidationProblem>> CreateJobAd(
      [FromBody] JobAdCreateDto ad,
      JobVacancyDbContext db,
      IHttpContextAccessor accessor,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken = default)
  {
    var company = await db.Companies
      .FirstOrDefaultAsync(x => x.Id == ad.CompanyId, cancellationToken);

    if (company is null)
    {
      logger.LogEntityNotFound(nameof(Company), ad.CompanyId);
      return TypedResults.ValidationProblem([new KeyValuePair<string, string[]>(nameof(JobAdCreateDto.CompanyId), ["No company with that id exists."])]);
    }

    var jobAd = ad.ToEntity(company);
    db.JobAds.Add(jobAd);
    await db.SaveChangesAsync(cancellationToken);

    var jobAdAndUser = await GetUserJobAdMapped(jobAd.Id, accessor, db, cancellationToken);

    return TypedResults.CreatedAtRoute(jobAdAndUser.ToResponseDto(), nameof(GetJobAd), new { id = jobAd.Id });
  }

  private static async Task<Results<Ok<JobAdResponseDto>, NotFound>> UpdateJobAd(
      Guid id,
      JobAdUpdateDto dto,
      IHttpContextAccessor accessor,
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken = default)
  {
    var jobAdAndUser = await GetUserJobAdMapped(id, accessor, db, cancellationToken);

    if (jobAdAndUser.JobAd is null)
    {
      logger.LogEntityNotFound(nameof(JobAd), id);
      return TypedResults.NotFound();
    }

    jobAdAndUser.JobAd.Apply(dto);
    await db.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok(jobAdAndUser.ToResponseDto());
  }

  private static async Task<Results<Ok, NotFound>> DeleteJobAd(
      [FromRoute] Guid id,
      JobVacancyDbContext db,
      ILogger<V2JobAdEndpoints> logger,
      CancellationToken cancellationToken = default)
  {
    var jobAd = await db.JobAds.FindAsync(id,cancellationToken);

    if (jobAd is null)
    {
      logger.LogEntityNotFound(nameof(JobAd), id);
      return TypedResults.NotFound();
    }

    db.JobAds.Remove(jobAd);
    await db.SaveChangesAsync(cancellationToken);
    return TypedResults.Ok();
  }

  private static async Task<Results<NoContent, NotFound, UnprocessableEntity>> FavoriteJobAd(
      Guid id,
      JobVacancyDbContext ctx,
      IHttpContextAccessor accessor,
      bool favorite = true,
      CancellationToken cancellationToken = default)
  {
    var jobAd = await ctx.JobAds.FindAsync(id, cancellationToken);
    if (jobAd is null)
    {
      return TypedResults.NotFound();
    }

    var user = await GetUser(accessor, ctx, cancellationToken);

    if (user is null)
    {
      user = accessor.HttpContext!.User.Claims.ToList().MapToUser();
      ctx.Users.Add(user);
      await ctx.SaveChangesAsync(cancellationToken);
    }

    var userFavorite = await ctx.UserFavoriteJobAd
        .FirstOrDefaultAsync(x => x.JobAd.Id == id && x.User.Id == user.Id, cancellationToken);
    if (userFavorite is null)
    {
      userFavorite = new UserJobAd{ User = user, JobAd = jobAd, IsFavorite = favorite };
      ctx.UserFavoriteJobAd.Add(userFavorite);
      await ctx.SaveChangesAsync(cancellationToken);
    }

    return TypedResults.NoContent();
  }

  private static async Task<User?> GetUser(
    IHttpContextAccessor accessor,
    JobVacancyDbContext ctx,
    CancellationToken cancellationToken = default)
  {
    var userId = accessor.HttpContext?.User.Claims.GetUserIdFromClaims();

    return userId is null ?
      null : await ctx.Users.FindAsync(userId, cancellationToken);
  }

  private static async Task<UserJobAd> GetUserJobAdMapped(
    Guid jobAdId,
    IHttpContextAccessor accessor,
    JobVacancyDbContext db,
    CancellationToken ct = default)
  {
    var userId = accessor.HttpContext?.User.Claims.GetUserIdFromClaims();
    var userJobAd = await db.UserFavoriteJobAd
      .Include(x => x.JobAd.Company)
      .FirstOrDefaultAsync(x => x.JobAd.Id == jobAdId && x.User.Id == userId, ct);

    return userJobAd ?? new UserJobAd
    {
      User = await db.Users.FindAsync(userId, ct),
      JobAd = await db.JobAds
        .Include(x=>x.Company)
        .Where(x=> x.Id == jobAdId)
        .FirstOrDefaultAsync(ct),
      IsFavorite = false
    };
  }

}

namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto;
using Adaptit.Training.JobVacancy.Web.Models.Dto.User;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.Helpers;
using Adaptit.Training.JobVacancy.Web.Server.Services;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class V2UserEndpoints()
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("users").WithTags("User");

    group.MapGet(string.Empty, GetAllUsers);
    group.MapGet("{id:guid}", GetUserById).WithName("GetUserById");
    group.MapPost(string.Empty, CreateUser);
    group.MapPut("{id:guid}", UpdateUser);
    group.MapDelete("{id:guid}", DeleteUser);

    return endpoint;
  }

  public static async Task<Results<Ok<PagedList<UserReturnDto>>, BadRequest<string>>> GetAllUsers(
    JobVacancyDbContext dbContext,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken,
    int page = 1,
    int pageSize = 10)
  {
    if (page < 1 || pageSize < 1 || pageSize > 100)
    {
      return TypedResults.BadRequest("Invalid page or pageSize");
    }

    var query = await dbContext.Users
      .OrderBy(u => u.Id)
      .ToPagedListAsync(u => u.ToUserReturnDto(),page, pageSize, cancellationToken);

    return TypedResults.Ok(query);
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> GetUserById(
    Guid id,
    JobVacancyDbContext dbContext,
    BlobStorageService blobStorageService,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.Where(u => u.Id == id)
      .Include(u => u.Resumes)
      .Select(u => u.ToUserReturnDto())
      .FirstOrDefaultAsync(cancellationToken);

    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);

      return TypedResults.NotFound();
    }

    return TypedResults.Ok(user);
  }

  public static async Task<Results<CreatedAtRoute<UserReturnDto>, BadRequest>> CreateUser(
    UserCreateDto userCreateDto,
    JobVacancyDbContext dbContext,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    if (userCreateDto.Name == string.Empty || userCreateDto.Surname == string.Empty)
    {
      return TypedResults.BadRequest();
    }

    var user = userCreateDto.ToUser();

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();

    return TypedResults.CreatedAtRoute(dto, nameof(GetUserById), new {id = dto.Id});
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> UpdateUser(
    UserUpdateDto userUpdateDto,
    Guid id,
    JobVacancyDbContext dbContext,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.FindAsync(id, cancellationToken);
    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);

      return TypedResults.NotFound();
    }

    user.Name = userUpdateDto.Name;
    user.LastName = userUpdateDto.Surname;
    user.Email = userUpdateDto.Email;

    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();

    return TypedResults.Ok(dto);
  }

  public static async Task<Results<NoContent, NotFound, InternalServerError>> DeleteUser(
    Guid id,
    JobVacancyDbContext dbContext,
    ILogger<V2UserEndpoints> logger,
    BlobStorageService blobStorageService,
    CancellationToken cancellationToken)
  {
    logger.LogDeletingEntityOfTypeWithId(nameof(User), id);
    var user = await dbContext.Users.Include(u => u.Resumes).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);

      return TypedResults.NotFound();
    }

    if (user.Resumes.Count > 0)
    {
      var deleted = await blobStorageService.DeleteAllResumesAsync(id, cancellationToken);

      if (!deleted)
      {
        return TypedResults.InternalServerError();
      }

      dbContext.Resumes.RemoveRange(user.Resumes);
    }

    dbContext.Users.Remove(user);
    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.NoContent();
  }
}

namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.User;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class V2UserEndpoints()
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("users").WithTags("User");

    group.MapGet("", GetAllUsers);
    group.MapGet("{id:guid}", GetUserById).WithName("GetUserById");
    group.MapPost(string.Empty, CreateUser);
    group.MapPost("upload-resume/{id:guid}", UploadUserResume).DisableAntiforgery();
    group.MapPut("{id:guid}", UpdateUser);
    group.MapDelete("{id:guid}", DeleteUser);

    return endpoint;
  }
  public static async Task<Ok<ICollection<UserReturnDto>>> GetAllUsers(JobVacancyDbContext dbContext, ILogger<V2UserEndpoints> logger, CancellationToken cancellationToken)
  {
    logger.LogGettingEntitiesOfType(nameof(User));
    var users = await dbContext.Users.ToListAsync(cancellationToken);
    var dtos = users.ToUserReturnDtoList();
    return TypedResults.Ok(dtos);
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> GetUserById(Guid id, JobVacancyDbContext dbContext,
    ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    logger.LogGettingEntityOfTypeWithId(nameof(User), id);
    var user = await dbContext.Users.FindAsync(id, cancellationToken);
    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);
      return TypedResults.NotFound();
    }

    var dto = user.ToUserReturnDto();
    return TypedResults.Ok(dto);
  }

  public static async Task<Results<CreatedAtRoute<UserReturnDto>, BadRequest>> CreateUser(UserCreateDto userCreateDto,
    JobVacancyDbContext dbContext, ILogger<V2UserEndpoints> logger, CancellationToken cancellationToken)
  {
    logger.LogCreatingEntityOfType(nameof(User));
    var user = userCreateDto.ToUser();

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();
    return TypedResults.CreatedAtRoute(dto, nameof(GetUserById), new {id = dto.Id});
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> UpdateUser(UserUpdateDto userUpdateDto, Guid id,
    JobVacancyDbContext dbContext, ILogger<V2UserEndpoints> logger, CancellationToken cancellationToken)
  {
    logger.LogUpdatingEntityOfTypeWithId(nameof(User), id);
    var user = await dbContext.Users.FindAsync(id, cancellationToken);
    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);
      return TypedResults.NotFound();
    }

    user.Name = userUpdateDto.Name;
    user.Surname = userUpdateDto.Surname;

    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();
    return TypedResults.Ok(dto);
  }

  public static async Task<Results<NoContent, NotFound>> DeleteUser(Guid id, JobVacancyDbContext dbContext, ILogger<V2UserEndpoints> logger,
    CancellationToken cancellationToken)
  {
    logger.LogDeletingEntityOfTypeWithId(nameof(User), id);
    var user = await dbContext.Users.FindAsync(id, cancellationToken);
    if (user == null)
    {
      logger.LogEntityNotFound(nameof(user), id);
      return TypedResults.NotFound();
    }
    dbContext.Users.Remove(user);
    await dbContext.SaveChangesAsync(cancellationToken);
    return TypedResults.NoContent();
  }

  public static async Task<Results<Ok, BadRequest<string>>> UploadUserResume(IFormFile file, CancellationToken cancellationToken)
  {
    if (file == null || file.Length == 0 || file.ContentType != "application/pdf")
    {
      return TypedResults.BadRequest("Need a pdf file");
    }
    // TODO: logiki gia save

    return TypedResults.Ok();
  }
}


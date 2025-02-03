namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models.Dto.User;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class V2UserEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("user").WithTags("User");

    group.MapGet("", GetAllUsers);
    group.MapGet("{id}", GetUserById);

    return endpoint;
  }
  public static async Task<Ok<ICollection<UserReturnDto>>> GetAllUsers(JobVacancyDbContext dbContext, CancellationToken cancellationToken)
  {
    var users = await dbContext.Users.ToListAsync(cancellationToken);
    var dtos = users.ToUserReturnDtoList();
    return TypedResults.Ok(dtos);
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> GetUserById(string id, JobVacancyDbContext dbContext,
    CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.FindAsync(id, cancellationToken);
    if (user == null)
    {
      return TypedResults.NotFound();
    }

    var dto = user.ToUserReturnDto();
    return TypedResults.Ok(dto);
  }

  public static async Task<Results<CreatedAtRoute<UserReturnDto>, BadRequest>> CreateUser(UserModifyDto userModifyDto,
    JobVacancyDbContext dbContext, CancellationToken cancellationToken)
  {
    userModifyDto.Id = Guid.NewGuid();

    var user = userModifyDto.ToUser();

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();
    return TypedResults.CreatedAtRoute(dto);
  }

  public static async Task<Results<Ok<UserReturnDto>, NotFound>> UpdateUser(UserModifyDto userModifyDto,
    JobVacancyDbContext dbContext, CancellationToken cancellationToken)
  {
    var user = await dbContext.Users.FindAsync(userModifyDto.Id, cancellationToken);
    if (user == null)
    {
      return TypedResults.NotFound();
    }
    user = userModifyDto.ToUser();
    dbContext.Users.Update(user);
    await dbContext.SaveChangesAsync(cancellationToken);
    var dto = user.ToUserReturnDto();
    return TypedResults.Ok(dto);
  }
}


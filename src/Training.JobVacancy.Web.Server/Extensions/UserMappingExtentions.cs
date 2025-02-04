namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.User;

public static class UserMappingExtentions
{
  public static UserReturnDto ToUserReturnDto(this User user)
  {
    ArgumentNullException.ThrowIfNull(user);

    var dto = new UserReturnDto
    {
      Id = user.Id,
      Name = user.Name,
      Surname = user.Surname,
      Resume = user.Resume,
    };
    return dto;
  }

  public static ICollection<UserReturnDto> ToUserReturnDtoList(this ICollection<User> users)
  {
    ArgumentNullException.ThrowIfNull(users);
    var dtos = users.Select(u => u.ToUserReturnDto()).ToList();
    return dtos;
  }

  public static User ToUser(this UserCreateDto userCreateDto)
  {
    ArgumentNullException.ThrowIfNull(userCreateDto);
    var user = new User
    {
      Id = Guid.NewGuid(),
      Name = userCreateDto.Name,
      Surname = userCreateDto.Surname,
      Resume = userCreateDto.Resume,
    };
    return user;
  }
}

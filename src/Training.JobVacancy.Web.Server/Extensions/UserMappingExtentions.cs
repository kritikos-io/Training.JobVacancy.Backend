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

  public static User ToUser(this UserModifyDto userModifyDto)
  {
    ArgumentNullException.ThrowIfNull(userModifyDto);
    var user = new User
    {
      Id = userModifyDto.Id,
      Name = userModifyDto.Name,
      Surname = userModifyDto.Surname,
      Resume = userModifyDto.Resume,
    };
    return user;
  }

  public static User ToUser(this UserReturnDto userReturnDto)
  {
    ArgumentNullException.ThrowIfNull(userReturnDto);
    var user = new User
    {
      Id = userReturnDto.Id,
      Name = userReturnDto.Name,
      Surname = userReturnDto.Surname,
      Resume = userReturnDto.Resume,
    };
    return user;
  }
}

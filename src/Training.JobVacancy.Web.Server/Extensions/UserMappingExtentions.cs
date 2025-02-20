namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.User;

public static class UserMappingExtentions
{
  public static UserReturnDto ToUserReturnDto(this User user)
  {
    var dto = new UserReturnDto
    {
      Id = user.Id,
      Name = user.Name,
      Surname = user.Surname,
      Resumes = user.Resumes.ToResumeReturnDtos(),
    };

    return dto;
  }

  public static List<UserReturnDto> ToUserReturnDtoList(this List<User> users)
  {
    var dtos = users.Select(u => u.ToUserReturnDto()).ToList();

    return dtos;
  }

  public static User ToUser(this UserCreateDto userCreateDto)
  {
    var user = new User
    {
      Name = userCreateDto.Name,
      Surname = userCreateDto.Surname,
    };

    return user;
  }
}

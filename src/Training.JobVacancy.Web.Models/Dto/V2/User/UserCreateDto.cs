namespace Adaptit.Training.JobVacancy.Web.Models.Dto.User;

using System.ComponentModel.DataAnnotations;

public class UserCreateDto
{
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  [EmailAddress]
  public string Email { get; set; } = string.Empty;
}

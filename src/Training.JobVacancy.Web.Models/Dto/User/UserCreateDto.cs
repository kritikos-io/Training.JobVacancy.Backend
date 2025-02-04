namespace Adaptit.Training.JobVacancy.Web.Models.Dto.User;

public class UserCreateDto
{
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  public byte[] Resume { get; set; } = [];
}

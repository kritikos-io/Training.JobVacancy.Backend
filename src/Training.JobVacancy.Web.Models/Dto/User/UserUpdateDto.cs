namespace Adaptit.Training.JobVacancy.Web.Models.Dto.User;

public class UserUpdateDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  public string Resume { get; set; } = string.Empty;
}

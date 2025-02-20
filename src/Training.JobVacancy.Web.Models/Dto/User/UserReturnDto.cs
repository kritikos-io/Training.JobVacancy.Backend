namespace Adaptit.Training.JobVacancy.Web.Models.Dto.User;

using Adaptit.Training.JobVacancy.Web.Models.Dto.Resume;

public class UserReturnDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  public List<ResumeReturnDto> Resumes { get; set; } = [];
}

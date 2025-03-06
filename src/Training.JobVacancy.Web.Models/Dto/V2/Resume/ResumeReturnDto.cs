namespace Adaptit.Training.JobVacancy.Web.Models.Dto.Resume;

public class ResumeReturnDto
{
  public int Id { get; set; }
  public Uri DownloadUrl { get; set; } = null!;
  public bool IsInUse { get; set; }
  public DateTime CreatedAt { get; set; }
}

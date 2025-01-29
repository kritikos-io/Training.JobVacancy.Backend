namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public class EntryDetailsDto
{
  public Guid Uuid { get; set; }

  public EntryStatus Status { get; set; }

  public string Title { get; set; } = string.Empty;

  public string BusinessName { get; set; } = string.Empty;

  public string Municipal { get; set; } = string.Empty;
}

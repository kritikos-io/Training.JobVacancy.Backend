namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public enum EntryStatus
{
  None = 0,
  Active = 1,
  Inactive = Active << 1,
}

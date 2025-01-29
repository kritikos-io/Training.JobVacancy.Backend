namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public enum Source
{
  None = 0,
  ImportApi = 1,
  Stillingsregistrering = ImportApi << 1,
}

namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public enum CategoryType
{
  None = 0,
  Esco = 1,
  Janzz = Esco << 1,
  Styrk08 = Janzz << 1,
}

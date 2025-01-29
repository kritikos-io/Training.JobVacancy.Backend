namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public class CategoryDto
{
  public CategoryType CategoryType { get; set; }

  public string Code { get; set; } = string.Empty;

  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public decimal Score { get; set; }
}

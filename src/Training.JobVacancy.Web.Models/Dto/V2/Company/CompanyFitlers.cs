namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

public class CompanyFilters
{
  public string? Name { get; set; }

  public AddressDto? Address { get; set; }

  public bool? HasActiveJobs { get; set; }
}

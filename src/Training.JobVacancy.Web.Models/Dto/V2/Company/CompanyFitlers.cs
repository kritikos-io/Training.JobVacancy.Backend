namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

public class CompanyFilters
{
  public string? Name { get; set; }

  public string? Vat { get; set; }

  public string? PhoneNumber { get; set; }

  public AddressDto? Address { get; set; }

  public int PageNumber { get; set; } = 1;

  public int PageSize { get; set; } = 20;

}

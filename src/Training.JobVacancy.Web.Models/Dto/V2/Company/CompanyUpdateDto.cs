namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

public class CompanyUpdateDto
{
  public string? Name { get; set; }

  public Uri? Website { get; set; }

  public string? Vat { get; set; }

  public Uri? LogoUrl { get; set; }

  public AddressUpdateDto? Address { get; set; }

  public bool? Sponsored { get; set; }

  public int? TotalJobsAdvertised { get; set; }

  public string? PhoneNumber { get; set; }

}

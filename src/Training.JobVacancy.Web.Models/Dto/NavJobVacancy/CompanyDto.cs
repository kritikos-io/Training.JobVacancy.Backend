namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

public class CompanyDto
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public Uri Website { get; set; } = new Uri(string.Empty);

  public string Vat { get; set; } = string.Empty;

  public Uri LogoUrl { get; set; } = new Uri(string.Empty);

  public AddressDto Address { get; set; } = new AddressDto();

  public bool Sponsored { get; set; } = false;

  public int TotalJobsAdvertised { get; set; } = 0;

}

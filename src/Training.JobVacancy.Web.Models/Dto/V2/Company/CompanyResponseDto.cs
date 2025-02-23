namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

public class CompanyResponseDto
{
  public Guid Id { get; set; }

  public string Name { get; set; }

  public Uri? Website { get; set; }

  public string? Vat { get; set; }

  public Uri? LogoUrl { get; set; }

  public AddressDto? Address { get; set; }

  public bool? Sponsored { get; set; }

  public IReadOnlyList<JobAdDto> JobAds { get; set; } = [];

  public string? PhoneNumber { get; set; }

}

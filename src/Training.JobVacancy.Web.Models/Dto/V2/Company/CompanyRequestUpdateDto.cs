namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

using System.ComponentModel.DataAnnotations;

public class CompanyRequestUpdateDto
{

  [Required(AllowEmptyStrings = false)]
  public string Name { get; set; }

  public Uri? Website { get; set; }

  [Required(AllowEmptyStrings = false)]
  public string Vat { get; set; }

  public Uri? LogoUrl { get; set; }

  public AddressDto? Address { get; set; }

  [Required(AllowEmptyStrings = false)]
  public bool Sponsored { get; set; }

  public string? PhoneNumber { get; set; }

}

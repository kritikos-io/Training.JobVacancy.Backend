namespace Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using System.Text.Json.Serialization;

public class CompanyDto
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;
  [JsonPropertyName("website")]
  public Uri? Website { get; set; }

  public string Vat { get; set; } = string.Empty;

  [JsonPropertyName("logoUrl")]
  public Uri? LogoUrl { get; set; }

  public AddressDto Address { get; set; } = new AddressDto();

  public bool Sponsored { get; set; } = false;

  public int TotalJobsAdvertised { get; set; } = 0;

  public string PhoneNumber { get; set; } = string.Empty;

}

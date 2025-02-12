namespace Adaptit.Training.JobVacancy.Web.Models.Entities;

public class Company
{
  public Guid Id { get; set; }

  public required string Name { get; set; }

  public Uri? Website { get; set; }

  public string Vat { get; set; } = string.Empty;

  public Uri? LogoUrl { get; set; }

  public Address Address { get; set; } = new Address();

  public bool Sponsored { get; set; } = false;

  public int TotalJobsAdvertised { get; set; } = 0;

  public string PhoneNumber { get; set; } = string.Empty;
}

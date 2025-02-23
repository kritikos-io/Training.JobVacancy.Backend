#nullable enable
namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;
using System.Collections.Generic;

public class Company
{
  public Guid Id { get; set; }

  public IReadOnlyCollection<JobAd> JobAds = [];

  public string Name { get; set; }

  public Uri? Website { get; set; }

  public string Vat { get; set; }

  public Uri? LogoUrl { get; set; }

  public Address Address { get; set; } = new Address();

  public bool Sponsored { get; set; }

  public string? PhoneNumber { get; set; }
}

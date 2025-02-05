namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;

public class JobAd
{
  public Guid Id { get; set; }

  public JobType Type { get; set; }

  public string SalaryRange { get; set; }

  public string Description { get; set; }

  public bool Favorite { get; set; }

  public string Location { get; set; }

  public DateTimeOffset CreatedAt { get; set; }

  public DateTimeOffset ExpiresAt { get; set; }

  public JobExperienceLevel Level { get; set; }
}

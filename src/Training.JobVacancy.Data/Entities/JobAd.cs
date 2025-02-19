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

  public DateTime CreatedAt { get; set; }

  public DateTime ExpiresAt { get; set; }

  public JobExperienceLevel Level { get; set; }
}

namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;

public class JobAd
{
  private Guid Id { get; set; }

  private JobType Type { get; set; }

  private string SalaryRange { get; set; }

  private string Description { get; set; }

  private bool Favorite { get; set; }

  private string Location { get; set; }

  private DateTime CreatedAt { get; set; }

  private DateTime ExpiresAt { get; set; }

  private JobExperienceLevel Level { get; set; }
}

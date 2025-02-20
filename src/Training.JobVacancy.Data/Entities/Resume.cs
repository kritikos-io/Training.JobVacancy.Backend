namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;

public class Resume
{
  public int Id { get; init; }
  public Uri DownloadUrl { get; set; } = null!;
  public bool IsInUse { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public User User { get; set; } = null!;
}

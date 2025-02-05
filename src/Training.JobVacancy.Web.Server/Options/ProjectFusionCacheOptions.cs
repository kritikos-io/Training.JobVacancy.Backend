namespace Adaptit.Training.JobVacancy.Web.Server.Options;

using System.ComponentModel.DataAnnotations;

public class ProjectFusionCacheOptions
{
  internal const string Section = "FusionCache";

  [Required]
  public int DefaultDurationMinutes { get; set; }
  [Required]
  public int JitterMaxDurationMs { get; set; }
  public string? RedisConnectionString { get; set; }
}

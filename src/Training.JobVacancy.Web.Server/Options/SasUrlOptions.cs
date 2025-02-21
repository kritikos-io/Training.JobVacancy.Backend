namespace Adaptit.Training.JobVacancy.Web.Server.Options;

using System.ComponentModel.DataAnnotations;

public class SasUrlOptions
{
  internal const string Section = "SasTokenSettings";

  [Required]
  [Range(1,180, ErrorMessage = "ValidForMinutes must be between 1 and 180")]
  public int ValidForMinutes { get; set; }
}

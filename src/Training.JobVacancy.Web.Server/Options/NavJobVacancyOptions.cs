namespace Adaptit.Training.JobVacancy.Web.Server.Options;

using System.ComponentModel.DataAnnotations;

public class NavJobVacancyOptions
{
  internal const string Section = "NavJobVacancy";

  [Required]
  public Uri BaseAddress { get; set; }

  public bool UseSandboxKey { get; set; }

  public string ApiKey { get; set; }
}

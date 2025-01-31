namespace Adaptit.Training.JobVacancy.Web.Server.Options;

using System.ComponentModel.DataAnnotations;

public class JobVacancyAuthenticationOptions
{
  public const string Section = "Authentication:OpenId";

  [Required(AllowEmptyStrings = false)]
  [Url]
  public string Authority { get; set; }

  public string ClientId { get; set; } = string.Empty;

  public string Audience { get; set; } = "account";
}

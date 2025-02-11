namespace Adaptit.Training.JobVacancy.Web.Models.Dto.V2;

using Adaptit.Training.JobVacancy.Data.Entities;

public class JobAdFilters
{
  public JobType? Type {get; set;}
  public bool? Favorite {get; set;}
  public double? Salary {get; set;}
  public DateTime? Created {get; set;}
  public DateTime? Expires {get; set;}
  public string? Description {get; set;}
}

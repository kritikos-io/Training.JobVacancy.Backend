namespace Adaptit.Training.JobVacancy.Web.Models;

public class PageList<TSourse>
{
  public List<TSourse> entries { get; set; } = new List<TSourse>();
  public int total { get; set; }
}

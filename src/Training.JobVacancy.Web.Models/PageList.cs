namespace Adaptit.Training.JobVacancy.Web.Models;

public class PageList<TSourse>(int page, int pageSize)
{
  public List<TSourse> pageList { get; set; } = new List<TSourse>();

  public int page { get; set; } = page;

  public int pageSize { get; set; } = pageSize;

  public int total { get; set; }
}

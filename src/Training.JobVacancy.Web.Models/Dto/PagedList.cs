namespace Adaptit.Training.JobVacancy.Web.Models.Dto;

public class PagedList<TSource>
{
  public List<TSource> Items { get; set; } = [];

  public int TotalItems { get; set; }

  public int CurrentPage { get; set; }

  public int TotalPages { get; set; }
}

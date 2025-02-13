namespace Adaptit.Training.JobVacancy.Web.Models.Dto;

public class PagedList<T>
{
  public required List<T> Items { get; init; }
  public required int Page { get; init; }
  public required int PageSize { get; init; }
  public required int TotalItemsCount { get; init; }

  public bool HasNextPage => Page * PageSize < TotalItemsCount;

  public bool HasPreviousPage => Page > 1;
}

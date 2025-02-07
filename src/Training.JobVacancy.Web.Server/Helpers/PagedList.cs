namespace Adaptit.Training.JobVacancy.Web.Server.Helpers;

using Microsoft.EntityFrameworkCore;

public class PagedList<T>
{
  private PagedList(List<T> items,int page, int pageSize, int totalCount)
  {
    Items = items;
    Page = page;
    PageSize = pageSize;
    TotalEntitiesCount = totalCount;
  }
  public List<T> Items { get; }
  public int Page { get; set; }
  public int PageSize { get; }
  public int TotalEntitiesCount { get; }

  public bool HasNextPage => Page * PageSize < TotalEntitiesCount;

  public bool HasPreviousPage => Page > 1;

  public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
  {
    int totalCount = await query.CountAsync();
    var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    return new(items, page, pageSize, totalCount);
  }
}

namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using System.ComponentModel;
using System.Linq.Expressions;

using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;

using Microsoft.EntityFrameworkCore;

public static class LinqExtensions
{
  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    => condition
      ? source.Where(predicate)
      : source;

  public static IOrderedQueryable OrderBy<TSource, TKey>(this IQueryable<TSource> source, ListSortDirection direction, Expression<Func<TSource, TKey>> selector)
    => direction == ListSortDirection.Ascending
      ? source.OrderBy(selector)
      : source.OrderByDescending(selector);

  public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> selector)
    => condition
      ? source.OrderBy(selector)
      : source;

  public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, ListSortDirection direction, Expression<Func<TSource, TKey>> selector)
    => !condition
      ? source
      : direction == ListSortDirection.Ascending
        ? source.OrderBy(selector)
        : source.OrderByDescending(selector);

  public static IQueryable<T> TakeIf<T>(this IQueryable<T> source, bool condition, int amount)
    => condition
      ? source.Take(amount)
      : source;

  public static async Task<PageList<T>> Page<T>(this IOrderedQueryable<T>source, CancellationToken cancellationToken,int pageNumber = 1, int pageSize = 20)
  {
    var totalEntries = await source.CountAsync(cancellationToken: cancellationToken);
    var pageList = new PageList<T>()
    {
      entries = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken),
      total = totalEntries
    };

    return pageList;
  }

  public static async Task<PageList<TDestination>> PageAsync<TSource,TDestination>(
    this IOrderedQueryable<TSource>source,
    Expression<Func<TSource,TDestination>> mapper,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
  {
    var totalEntries = await source.CountAsync(cancellationToken: cancellationToken);
    var pageList = new PageList<TDestination>
    {
      entries = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(mapper).ToListAsync(cancellationToken),
      total = totalEntries
    };

    return pageList;
  }
}

namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using System.ComponentModel;
using System.Linq.Expressions;

using Adaptit.Training.JobVacancy.Web.Models.Dto;

using Microsoft.EntityFrameworkCore;

public static class LinqExtensions
{
  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    => condition
      ? source.Where(predicate)
      : source;

  public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, ListSortDirection direction, Expression<Func<TSource, TKey>> selector)
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

  public static IEnumerable<T> SkipIf<T>(this IEnumerable<T> source, bool condition, int amount)
    => condition
        ? source.Skip(amount)
        : source;

  public static async Task<PagedList<TDestination>> ToPagedListAsync<TSource, TDestination>(
      this IOrderedQueryable<TSource> source,
      Expression<Func<TSource, TDestination>> mapper,
      int pageNumber,
      int pageSize,
      CancellationToken cancellationToken = default)
  {
    if (!source.TryGetNonEnumeratedCount(out var count))
    {
      count = await source.CountAsync(cancellationToken);
    }

    var result = new PagedList<TDestination>
    {
      Items = await source
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Select(mapper)
          .ToListAsync(cancellationToken),
      TotalItems = count,
      CurrentPage = pageNumber,
      TotalPages = (int)Math.Ceiling(count / (double)pageSize),
    };

    return result;
  }

  public static async Task<PagedList<TSource>> ToPagedListAsync<TSource>(
      this IOrderedQueryable<TSource> source,
      int pageNumber,
      int pageSize,
      CancellationToken cancellationToken = default)
  {
    if (!source.TryGetNonEnumeratedCount(out var count))
    {
      count = await source.CountAsync(cancellationToken);
    }

    var result = new PagedList<TSource>
    {
      Items = await source
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync(cancellationToken),
      TotalItems = count,
      CurrentPage = pageNumber,
      TotalPages = (int)Math.Ceiling(count / (double)pageSize),
    };

    return result;
  }
}

namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using System.ComponentModel;
using System.Linq.Expressions;

using Adaptit.Training.JobVacancy.Web.Models;

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

  public static PageList<T> Page<T>(this IOrderedQueryable<T>source, int pageNumber = 1, int pageSize = 50)
  {
    PageList<T> pageList = new PageList<T>(pageNumber, pageSize);
    pageList.pageList = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    return pageList;
  }
}

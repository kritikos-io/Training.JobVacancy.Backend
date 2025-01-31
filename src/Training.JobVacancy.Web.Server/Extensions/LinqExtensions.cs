namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using System.ComponentModel;

public static class LinqExtensions
{
  public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    => condition
        ? source.Where(predicate)
        : source;

  public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, ListSortDirection direction, Func<TSource, TKey> selector)
    => direction == ListSortDirection.Ascending
        ? source.OrderBy(selector)
        : source.OrderByDescending(selector);

  public static IEnumerable<TSource> OrderByIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> selector)
    => condition
        ? source.OrderBy(selector)
        : source;

  public static IEnumerable<TSource> OrderByIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, ListSortDirection direction, Func<TSource, TKey> selector)
    => !condition
        ? source
        : direction == ListSortDirection.Ascending
            ? source.OrderBy(selector)
            : source.OrderByDescending(selector);

  public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> source, bool condition, int amount)
    => condition
        ? source.Take(amount)
        : source;

  public static IEnumerable<T> SkipIf<T>(this IEnumerable<T> source, bool condition, int amount)
    => condition
        ? source.Skip(amount)
        : source;
}

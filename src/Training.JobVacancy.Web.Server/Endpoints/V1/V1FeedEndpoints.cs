namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;

using System.ComponentModel;
using System.Globalization;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

public class V1FeedEndpoints
{

  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("feed")
        .WithTags("Feed");

    group.MapGet("", GetLatestFeedPage)
        .ProducesValidationProblem();

    group.MapGet("{id:guid}", GetFeedPage);

    return endpoint;
  }

  public static Results<Ok<FeedDto>, NotFound, ValidationProblem> GetLatestFeedPage(
      [FromHeader(Name = "If-Modified-Since")] string? modifiedSinceHeader,
      string? last,
      NavJobVacancyRepo repository,
      ILogger<V1FeedEndpoints> logger)
  {
    if (!DateTimeOffset.TryParseExact(
            modifiedSinceHeader,
            "R",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal,
            out var modifiedSince)
        && modifiedSinceHeader is not null)
    {

      logger.LogApiValidationException(nameof(modifiedSinceHeader), nameof(GetLatestFeedPage));
      return TypedResults.ValidationProblem([new KeyValuePair<string, string[]>("If-Modified-Since", ["Valid dates should be in RFC1123"])]);
    }

    var feeds = repository.Feeds
      .OrderBy(
        last is not null
          ? ListSortDirection.Descending
          : ListSortDirection.Ascending,
        x => x.Items.FirstOrDefault()?.ModifiedAt);

    var feed = feeds
      .WhereIf(
        modifiedSinceHeader is not null,
        x => x.Items.FirstOrDefault()?.ModifiedAt > modifiedSince)
      .FirstOrDefault();

    return TypedResults.Ok(feed);
  }

  public static async Task<Results<Ok<FeedDto>, NotFound>> GetFeedPage(
      Guid id,
      NavJobVacancyRepo repository,
      ILogger<V1FeedEndpoints> logger,
      HybridCache cache)
  {

    var feedPage = await cache.GetOrCreateAsync<FeedDto?>(
      $"{nameof(FeedDto)}:{id}",
      _ => ValueTask.FromResult(repository.Feeds.FirstOrDefault(x => x.Id == id)));

    if (feedPage is not null)
    {
      return TypedResults.Ok(feedPage);
    }

    logger.LogEntityNotFound(nameof(FeedDto), id);
    return TypedResults.NotFound();

  }
}

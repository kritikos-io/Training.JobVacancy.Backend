namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Hybrid;

public class V1FeedEntryEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("feedentry")
        .WithTags("Feed Entries");

    group.MapGet("{id:guid}", GetFeedEntry);

    return endpoint;
  }

  public static async Task<Results<Ok<EntryDto>, NotFound>> GetFeedEntry(
      Guid id,
      NavJobVacancyRepo repository,
      ILogger<V1FeedEntryEndpoints> logger,
      HybridCache cache,
      CancellationToken ct)
  {
    var entry = await cache.GetOrCreateAsync<EntryDto?>(
      $"{nameof(EntryDto)}:{id}",
       _ => ValueTask.FromResult(repository.Entries.FirstOrDefault(x => x.Uuid == id)),
      cancellationToken: ct);

    if (entry is not null)
    {
      return TypedResults.Ok(entry);
    }

    logger.LogEntityNotFound(nameof(EntryDto), id);
    return TypedResults.NotFound();
  }
}

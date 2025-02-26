namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;

using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using Adaptit.Training.JobVacancy.Web.Server.Helpers;
using Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Microsoft.AspNetCore.Http.HttpResults;


public class V1FeedEntryEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("feedentry")
        .WithTags("Feed Entries");

    group.MapGet("{id:guid}", GetFeedEntry);

    return endpoint;
  }

  public static Results<Ok<EntryDto>, NotFound> GetFeedEntry(
      Guid id,
      NavJobVacancyRepo repository,
      ILogger<V1FeedEntryEndpoints> logger)
  {
    var entry = repository.Entries.FirstOrDefault(x => x.Uuid == id);
    if (entry is null)
    {
      logger.LogEntityNotFound(nameof(EntryDto), id);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(entry);
  }
}

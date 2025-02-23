namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;

using global::Adaptit.Training.JobVacancy.Backend.Helpers;
using global::Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using global::Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Microsoft.AspNetCore.Http.HttpResults;

using LogTemplates = Adaptit.Training.JobVacancy.Web.Server.Helpers.LogTemplates;

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
      LogTemplates.LogEntityNotFound(logger, nameof(EntryDto), id);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(entry);
  }
}

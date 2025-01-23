namespace Adaptit.Training.JobVacancy.Backend.Endpoints;

using Adaptit.Training.JobVacancy.Backend.Dto;

using Microsoft.AspNetCore.Http.HttpResults;

public static class FeedEntryEndpoints
{
	static int entries = 10;
	static List<FeedEntry> feedEntry = Enumerable.Repeat(new FeedEntry(), entries).ToList();

	public static IEndpointRouteBuilder MapFeedEntryEndpoint(this IEndpointRouteBuilder endpoints)
	{
		var api = endpoints.MapGroup("/api/v1/feedentry")
			.WithOpenApi();

		api.MapGet("/{entryId:int}", GetFeedEntry);

		return endpoints;
	}

	public static async Task<Results<Ok<FeedEntry>, NotFound<string>>> GetFeedEntry(int entryId)
	{
		if (entryId > entries || entryId < 0)
			return TypedResults.NotFound("Could not find entry");

		return TypedResults.Ok(feedEntry[entryId]);
	}
}

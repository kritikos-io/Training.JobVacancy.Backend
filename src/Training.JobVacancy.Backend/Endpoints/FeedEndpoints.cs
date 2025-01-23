namespace Adaptit.Training.JobVacancy.Backend.Endpoints;

using Adaptit.Training.JobVacancy.Backend.Dto;

using Microsoft.AspNetCore.Http.HttpResults;

public static class FeedEndpoints
{
	private static int pages = 10;
	static List<Feed> feed = Enumerable.Repeat(new Feed(), 10).ToList();

	public static IEndpointRouteBuilder MapFeedEndpoint(this IEndpointRouteBuilder endpoints)
	{
		var api = endpoints.MapGroup("/api/v1/feed")
			.WithOpenApi();

		api.MapGet("",(string? last) => GetAllFeed(last));
		api.MapGet("/{feedPageId:int}", GetFeed);

		return endpoints;
	}

	public static async Task<Results<Ok<Feed>, NotFound<string>>> GetAllFeed(string? last)
	{
		if (feed.Count == 0)
			return TypedResults.NotFound("No pages available");

		return (last is "last") ? TypedResults.Ok(feed.Last()) : TypedResults.Ok(feed.First());
	}

	public static async Task<Results<Ok<Feed>, EmptyHttpResult, NotFound<string>>> GetFeed(int feedPageId)
	{
		if (feedPageId > pages || feedPageId < 1)
			return TypedResults.NotFound("Could not find page");

		return TypedResults.Ok(feed[feedPageId]);
	}
}

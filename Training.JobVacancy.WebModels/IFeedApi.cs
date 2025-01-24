using Adaptit.Training.JobVacancy.Backend.Dto;

using Refit;

namespace Adaptit.Training.JobVacancy.WebModels;

public interface IFeedApi
{
	[Get("/api/v1/feed")]
	Task<ApiResponse<Feed>> GetAllFeed(string? last);

	[Get("/api/v1/feed/{feedPageId:int)")]
	Task<ApiResponse<Feed>> GetFeed(int feedPageId);

	[Get("/api/v1/feedentry/{entryId:int}")]
	Task<ApiResponse<FeedEntry>> GetFeedEntry(int entryId);
}

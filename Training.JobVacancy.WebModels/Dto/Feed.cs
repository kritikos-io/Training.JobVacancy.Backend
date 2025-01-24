namespace Adaptit.Training.JobVacancy.Backend.Dto;

public class Feed
{
	public string Version { get; set; }
	public string Title { get; set; }
	public string HomePageUrl { get; set; }
	public string FeedUrl { get; set; }
	public string Description { get; set; }
	public string? NextUrl { get; set; }
	public int Id { get; set; }
	public int? NextId { get; set; }
	public List<FeedLine> Entries { get; set; }
}

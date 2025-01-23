namespace Adaptit.Training.JobVacancy.Backend.Dto;

public class FeedLine
{
	public string Id { get; set; }
	public string Url { get; set; }
	public string Title { get; set; }
	public string ContentText { get; set; }
	public DateTime? DateModified { get; set; }
	public FeedEntry FeedEntry { get; set; }
}

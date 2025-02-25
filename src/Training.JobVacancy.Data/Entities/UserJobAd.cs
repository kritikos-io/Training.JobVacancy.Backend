namespace Adaptit.Training.JobVacancy.Data.Entities;

public class UserJobAd
{
  public User User { get; set; }

  public JobAd JobAd { get; set; }

  public bool IsFavorite { get; set; }
}

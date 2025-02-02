namespace Adaptit.Training.JobVacancy.Data;

using Adaptit.Training.JobVacancy.Data.Entities;

using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
  public DbSet<User> Users { get; set; }
}

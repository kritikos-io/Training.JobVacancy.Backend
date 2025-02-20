namespace Adaptit.Training.JobVacancy.Data;

using Adaptit.Training.JobVacancy.Data.Entities;

using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
  public DbSet<User> Users { get; set; }
  public DbSet<Resume> Resumes { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Resume>()
      .HasOne(r => r.User)
      .WithMany(u => u.Resumes)
      .OnDelete(DeleteBehavior.Cascade);
  }
}

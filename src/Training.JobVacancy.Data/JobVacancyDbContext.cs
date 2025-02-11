namespace Adaptit.Training.JobVacancy.Data;

using Adaptit.Training.JobVacancy.Data.Entities;

using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
  public DbSet<JobAd> JobAds { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<JobAd>(entity =>
    {
      entity.ToTable("JobAd");
      entity.HasKey(j => j.Id);
      entity.Property(j => j.Type)
        .HasConversion<string>();
      entity.Property(j => j.Level)
        .HasConversion<string>();

      entity.HasIndex(j => new {j.Description})
        .HasMethod("GIN")
        .IsTsVectorExpressionIndex("english");
    });
  }
}

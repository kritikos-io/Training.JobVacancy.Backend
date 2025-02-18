namespace Adaptit.Training.JobVacancy.Data;

using Adaptit.Training.JobVacancy.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
  public DbSet<Company> Companies { get; set; }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder) =>

    modelBuilder
      .Entity<Company>(e =>
      {
        e.HasKey(c => c.Id);

        e.Property(c => c.Name)
          .IsRequired()
          .HasMaxLength(50);

        e.HasIndex(c => c.Name)
          .IsUnique();

        e.Property(c => c.PhoneNumber)
          .HasMaxLength(10);

        e.OwnsOne(c => c.Address);
      });
}

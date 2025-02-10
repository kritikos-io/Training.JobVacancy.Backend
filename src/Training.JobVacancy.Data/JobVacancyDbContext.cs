namespace Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder) =>

    modelBuilder.Entity<Company>(e =>
    {
      e.HasKey(c => c.Id);

      e.Property(c => c.Name)
        .IsRequired()
        .HasMaxLength(50);

      e.Property(c => c.PhoneNumber)
        .HasAnnotation("PhoneNumberFormat", @"^[0-9(){}\-\+ ]+$")
        .HasMaxLength(20);

      e.HasOne<Address>()
        .WithMany()
        .OnDelete(DeleteBehavior.NoAction);
    });

}

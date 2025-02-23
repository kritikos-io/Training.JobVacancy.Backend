﻿namespace Adaptit.Training.JobVacancy.Data;

using Adaptit.Training.JobVacancy.Data.Entities;

using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
  public DbSet<Company> Companies { get; set; }

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

      entity.HasIndex(j => new { j.Description })
          .HasMethod("GIN")
          .IsTsVectorExpressionIndex("english");
    });

    modelBuilder
        .Entity<Company>(e =>
        {
          e.HasKey(c => c.Id);

          e.Property(c => c.Name)
              .IsRequired()
              .HasMaxLength(50);

          e.HasIndex(c => c.Vat)
              .IsUnique();

          e.Property(c => c.PhoneNumber)
              .HasMaxLength(10);

          e.OwnsOne(c => c.Address);
        });
  }
}

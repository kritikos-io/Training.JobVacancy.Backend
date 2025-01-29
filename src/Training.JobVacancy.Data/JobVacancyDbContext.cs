namespace Adaptit.Training.JobVacancy.Data;

using Microsoft.EntityFrameworkCore;

public class JobVacancyDbContext(DbContextOptions<JobVacancyDbContext> options)
    : DbContext(options)
{
}

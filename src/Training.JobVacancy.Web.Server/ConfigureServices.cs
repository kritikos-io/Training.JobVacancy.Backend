namespace Adaptit.Training.JobVacancy.Web.Server;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models;
using Adaptit.Training.JobVacancy.Web.Server.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Refit;

public static class ConfigureServices
{
  public static void AddJobVacancyServices(this WebApplicationBuilder builder)
  {
    builder.AddNavJobVacancyClient();
    builder.Services.AddDbContext<JobVacancyDbContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("JobVacancyDatabase"))
        .EnableSensitiveDataLogging());
  }

  public static void AddNavJobVacancyClient(this WebApplicationBuilder builder)
  {
    builder.Services.AddOptionsWithValidateOnStart<NavJobVacancyOptions>()
        .BindConfiguration(NavJobVacancyOptions.Section)
        .ValidateDataAnnotations();

    builder.Services.AddRefitClient<INavJobVacancy>(sp =>
        {
          var apiOptions = sp.GetRequiredService<IOptions<NavJobVacancyOptions>>().Value;
          return new RefitSettings { AuthorizationHeaderValueGetter = (message, token) => Task.FromResult(apiOptions.ApiKey), };
        })
        .ConfigureHttpClient((sp, client) =>
        {
          var settings = sp.GetRequiredService<IOptions<NavJobVacancyOptions>>().Value;
          client.BaseAddress = new Uri(settings.BaseAddress.ToString());
        });
  }
}

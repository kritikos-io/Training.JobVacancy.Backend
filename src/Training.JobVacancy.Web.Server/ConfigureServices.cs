namespace Adaptit.Training.JobVacancy.Web.Server;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models;
using Adaptit.Training.JobVacancy.Web.Server.Options;
using Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Asp.Versioning;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Refit;

public static class ConfigureServices
{
  public static void AddJobVacancyServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddProblemDetails();

    builder.AddNavJobVacancyClient();
    builder.AddNavJobRepository();

    builder.AddApiDocumentation();
    builder.AddApiVersioning();

    builder.Services.AddDbContext<JobVacancyDbContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("JobVacancyDatabase"))
        .EnableSensitiveDataLogging());
  }

  public static void AddApiDocumentation(this WebApplicationBuilder builder)
  {
    builder.Services.AddOpenApi();
  }

  public static void AddApiVersioning(this WebApplicationBuilder builder)
  {
    builder.Services.AddApiVersioning(options =>
        {
          options.DefaultApiVersion = new ApiVersion(1);
          options.ReportApiVersions = true;
          options.AssumeDefaultVersionWhenUnspecified = true;
          options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
          options.GroupNameFormat = "'v'V";
          options.SubstituteApiVersionInUrl = true;
          options.AssumeDefaultVersionWhenUnspecified = true;
          options.RouteConstraintName = "apiVersion";
        });
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

  public static void AddNavJobRepository(this WebApplicationBuilder builder)
  {
    builder.Services.AddSingleton<NavJobVacancyRepo>(options =>
    {
      var seed = builder.Configuration.GetValue<int>("Seed", 32131095);
      return new NavJobVacancyRepo(seed);
    });
  }
}

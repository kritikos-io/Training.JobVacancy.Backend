﻿namespace Adaptit.Training.JobVacancy.Web.Server;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models;
using Adaptit.Training.JobVacancy.Web.Server.Helpers;
using Adaptit.Training.JobVacancy.Web.Server.Middlewares;
using Adaptit.Training.JobVacancy.Web.Server.OpenApi.DocumentTransformers;
using Adaptit.Training.JobVacancy.Web.Server.OpenApi.OperationTransformers;
using Adaptit.Training.JobVacancy.Web.Server.OpenApi.SchemaTransformers;
using Adaptit.Training.JobVacancy.Web.Server.Options;
using Adaptit.Training.JobVacancy.Web.Server.Repositories;
using Adaptit.Training.JobVacancy.Web.Server.Services;

using Asp.Versioning;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Refit;

public static class ConfigureServices
{
  public static void AddJobVacancyServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddProblemDetails();
    builder.AddJobVacancyAuthentication();

    builder.AddNavJobVacancyClient();
    builder.AddNavJobRepository();

    builder.AddApiDocumentation();
    builder.AddApiVersioning();

    builder.AddCentralizedLogging();

    builder.AddBlobStorageOptions();

    builder.Services.AddDbContext<JobVacancyDbContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("JobVacancyDatabase"))
        .EnableSensitiveDataLogging());
  }

  public static void AddJobVacancyAuthentication(this WebApplicationBuilder builder)
  {
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddOptionsWithValidateOnStart<JobVacancyAuthenticationOptions>()
      .BindConfiguration(JobVacancyAuthenticationOptions.Section)
      .ValidateDataAnnotations();

    builder.Services.AddAuthentication()
      .AddJwtBearer("openid",
        options =>
        {
          var realmSettings = new JobVacancyAuthenticationOptions();
          builder.Configuration.GetRequiredSection(JobVacancyAuthenticationOptions.Section)
            .Bind(realmSettings);

          options.Authority = realmSettings.Authority;
          options.MetadataAddress = $"{options.Authority}/.well-known/openid-configuration";

          options.RequireHttpsMetadata = true;
          options.SaveToken = true;
          options.MapInboundClaims = false;

          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = realmSettings.Authority,
            ValidAudience = "account",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "groups",
          };
        });

    builder.Services.AddAuthorizationBuilder()
      .AddDefaultPolicy("default",
        policy => policy
          .AddAuthenticationSchemes("openid")
          .RequireAuthenticatedUser())
      .AddDefaultPolicy("admin",
          policy=>policy
              .RequireRole("Admin"));
  }

  public static void AddApiDocumentation(this WebApplicationBuilder builder)
  {
    builder.Services.AddOpenApi("v1",
      options =>
      {
        options.AddDocumentTransformer<ApiVersionDocumentTransformer>();
        options.AddDocumentTransformer<SecuritySchemeTransformer>();
        options.AddOperationTransformer<DeprecatedVersionTransformer>();
        options.AddOperationTransformer<AuthorizationCheckOperationTransformer>();
        options.AddSchemaTransformer<NullableSchemaTransformer>();
      });
    builder.Services.AddOpenApi("v2",
      options =>
      {
        options.AddDocumentTransformer<ApiVersionDocumentTransformer>();
        options.AddDocumentTransformer<SecuritySchemeTransformer>();
        options.AddOperationTransformer<DeprecatedVersionTransformer>();
        options.AddOperationTransformer<AuthorizationCheckOperationTransformer>();
        options.AddSchemaTransformer<NullableSchemaTransformer>();
      });
  }

  public static void AddApiVersioning(this WebApplicationBuilder builder)
  {
    builder.Services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        options.RouteConstraintName = "apiVersion";

        options.Policies
          .Sunset(1)
          .Effective(2025, 02, 15)
          .Link("api-policy.html")
          .Title("Versioning Policy")
          .Type("text/html")
          .Language("en");
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
        return new RefitSettings
        {
          AuthorizationHeaderValueGetter = (message, token) => Task.FromResult(apiOptions.ApiKey),
        };
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

  public static void AddMiddlewareServices(this WebApplicationBuilder builder) => builder.Services.AddTransient<CorrelationIdMiddleware>();

  public static void AddBlobStorageOptions(this WebApplicationBuilder builder)
  {
    builder.Services.AddOptions<BlobStorageOptions>()
      .Bind(builder.Configuration.GetSection(BlobStorageOptions.Section))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    builder.Services.AddAzureClients(cb => cb.AddBlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));
    builder.Services.AddScoped<BlobStorageService>();
  }

  public static void AddCentralizedLogging(this WebApplicationBuilder builder)
  {
    builder.Services.AddApplicationInsightsTelemetry();
    builder.Logging.AddSeq(builder.Configuration.GetSection("Seq"));
    builder.Logging.AddApplicationInsights(config =>
    {
      config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    }, options => options.FlushOnDispose = true);
  }
}

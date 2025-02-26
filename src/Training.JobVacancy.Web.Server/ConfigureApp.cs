namespace Adaptit.Training.JobVacancy.Web.Server;

using Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;
using Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Asp.Versioning;

public static class ConfigureApp
{
  public static WebApplication MapEndpoints(this WebApplication app)
  {
    var apiVersionSet = app.NewApiVersionSet()
        .HasDeprecatedApiVersion(new ApiVersion(1))
        .HasApiVersion(new ApiVersion(2))
        .ReportApiVersions()
        .Build();

    app.MapGroup("api/v{version:apiVersion}")
        .WithOpenApi()
        .WithApiVersionSet(apiVersionSet)
        .MapV1Endpoints();

    app.MapGroup("api/v{version:apiVersion}")
        .WithOpenApi()
        .WithApiVersionSet(apiVersionSet)
        .MapV2Endpoints();

    return app;
  }

  public static RouteGroupBuilder MapV1Endpoints(this RouteGroupBuilder endpoint)
  {

    var group = endpoint.MapToApiVersion(1);

    V1FeedEndpoints.Map(group);
    V1FeedEntryEndpoints.Map(group);

    return group;
  }

  public static RouteGroupBuilder MapV2Endpoints(this RouteGroupBuilder endpoint)
  {

    var group = endpoint.MapToApiVersion(2);

    V2CompanyEndpoints.Map(group);
    V2JobAdEndpoints.Map(group);

    return group;
  }
}

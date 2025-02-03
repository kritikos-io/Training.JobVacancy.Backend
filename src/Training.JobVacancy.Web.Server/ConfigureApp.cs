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

    var group = app.MapGroup("api/v{version:apiVersion}")
        .WithOpenApi()
        .WithApiVersionSet(apiVersionSet);

    group.MapV1Endpoints();
    group.MapV2Endpoints();

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
    V2UserEndpoints.Map(group);

    return group;
  }
}

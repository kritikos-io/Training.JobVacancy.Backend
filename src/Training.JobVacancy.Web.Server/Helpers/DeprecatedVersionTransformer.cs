namespace Adaptit.Training.JobVacancy.Web.Server.Helpers;

using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public class DeprecatedVersionTransformer(IApiVersionDescriptionProvider provider) : IOpenApiOperationTransformer
{
  /// <inheritdoc />
  public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
  {
    var doc = context.DocumentName;
    var version = provider.ApiVersionDescriptions.SingleOrDefault(x => x.GroupName == doc);
    if (version?.IsDeprecated ?? false)
    {
      operation.Deprecated = true;
    }
    return Task.CompletedTask;
  }
}

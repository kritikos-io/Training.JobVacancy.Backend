namespace Adaptit.Training.JobVacancy.Web.Server.OpenApi.OperationTransformers;

using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public class AuthorizationCheckOperationTransformer : IOpenApiOperationTransformer
{
  /// <inheritdoc />
  public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
  {
    var metadata = context.Description.ActionDescriptor.EndpointMetadata;
    operation.OperationId ??= metadata.OfType<MethodInfo>().First().Name;
    if (!metadata.OfType<IAuthorizeData>().Any())
    {
      return Task.CompletedTask;
    }

    operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
    operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

    var oAuthScheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "openid" } };

    operation.Security = [new() { [oAuthScheme] = ["openid"] }];

    return Task.CompletedTask;
  }
}

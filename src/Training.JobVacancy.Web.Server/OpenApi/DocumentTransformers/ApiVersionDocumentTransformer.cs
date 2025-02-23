namespace Adaptit.Training.JobVacancy.Web.Server.OpenApi.DocumentTransformers;

using System.Text;

using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;

public class ApiVersionDocumentTransformer(IApiVersionDescriptionProvider versionProvider) : IOpenApiDocumentTransformer
{
  private readonly IApiVersionDescriptionProvider versionProvider = versionProvider;

  /// <inheritdoc />
  public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
  {
    var apiDescription = versionProvider.ApiVersionDescriptions
        .SingleOrDefault(x => x.GroupName == context.DocumentName);
    if (apiDescription == null)
    {
      return Task.CompletedTask;
    }

    document.Info.Version = apiDescription.ApiVersion.ToString();
    document.Info.Title = "Job Vacancy API";
    document.Info.Description = BuildDescription(apiDescription, "Find your next job!");

    document.Info.License = new OpenApiLicense { Name = "Apache License, Version 2.0", Url = new Uri("https://opensource.org/license/apache-2-0"), };

    document.Info.Contact = new OpenApiContact { Name = "Training Team", Email = "noreply@example.com", };
    return Task.CompletedTask;
  }

  public static string BuildDescription(ApiVersionDescription api, string description)
  {
    var text = new StringBuilder(description);
    if (api.IsDeprecated)
    {
      if (text.Length > 0)
      {
        if (text[^1] != '.')
        {
          text.AppendLine();
        }

        text.Append(' ');
      }

      text.Append("This API version has been deprecated.");
    }

    if (api.SunsetPolicy is not { } policy)
    {
      return text.ToString();
    }

    if (policy.Date is { } when)
    {
      if (text.Length > 0)
      {
        text.Append(' ');

      }

      text.Append("This API version will be sunset on ")
          .Append(when.Date.ToShortDateString())
          .Append('.');
    }

    if (policy.HasLinks)
    {
      text.AppendLine();

      var rendered = false;
      foreach (var link in policy.Links.Where(l => l.Type == "text/html"))
      {
        if (!rendered)
        {
          text.AppendLine("For more information, please visit:");
          rendered = true;
        }

        text.Append("<li><a href=\"");
        text.Append(link.LinkTarget.OriginalString);
        text.Append("\">");
        text.Append(
            StringSegment.IsNullOrEmpty(link.Title)
                ? link.LinkTarget.OriginalString
                : link.Title.ToString());
        text.Append("</a></li>");
      }

      if (rendered)
      {
        text.AppendLine("</ul>");
      }
    }

    return text.ToString();
  }
}

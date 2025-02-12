namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using Adaptit.Training.JobVacancy.Web.Models.Entities;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class V2CompanyEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("company")
      .WithTags("Company");

    group.MapPost("", CreateCompany);
    group.MapGet("/{id:guid}", GetById).WithName("GetById");

    return endpoint;
  }

  public static CreatedAtRoute CreateCompany([FromBody] CompanyDto companyDto, JobVacancyDbContext dbContext)
  {

    var dto = companyDto.ToEntity();
    var entry = dbContext.Add(dto);
    dbContext.SaveChangesAsync();

    return TypedResults.CreatedAtRoute(
      routeName: "GetById",
      routeValues: entry.Entity.Id
    );
  }

  public static async Task<Results<NoContent, NotFound>> UpdateCompany([FromBody] CompanyDto companyDto, JobVacancyDbContext dbContext)
  {

    var entity = await dbContext.FindAsync<Company>(companyDto.Id);
    if (entity is null) TypedResults.NotFound();

    var enityFromDto = companyDto.ToEntity();

    foreach (var prop in typeof(Company).GetProperties())
    {
      var newValue = prop.GetValue(enityFromDto);
      if (newValue is not null)
      {
        typeof(Company).GetProperty(prop.Name)?.SetValue(entity, newValue);
      }
    }

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
  }

  public static async Task<Results<Ok<CompanyDto>, NotFound>> GetById(Guid companyId, JobVacancyDbContext dbContext)
  {
    var entity = await dbContext.FindAsync<Company>(typeof(Company),companyId);

    if (entity is null) TypedResults.NotFound();

    var dto = entity!.ToDto();
    return TypedResults.Ok(dto);
  }
}

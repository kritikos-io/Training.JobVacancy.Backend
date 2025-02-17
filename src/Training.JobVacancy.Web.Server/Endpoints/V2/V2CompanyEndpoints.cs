namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

public class V2CompanyEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("company")
      .WithTags("Company");

    group.MapPost("/search", Search).WithName("Search");
    group.MapGet("/{id:guid}", GetById).WithName("GetById");
    group.MapPost("", CreateCompany).WithName("CreateCompany");
    group.MapPatch("/{id:guid}", UpdateCompanyById).WithName("UpdateCompanyById");
    group.MapDelete("/{id:guid}", DeleteById).WithName("DeleteById");

    return endpoint;
  }

  public static async Task<Ok<PageList<CompanyDto>>> Search([FromBody] CompanyFilters? filters, JobVacancyDbContext dbContext,CancellationToken ct)
  {
    var companies = dbContext.Companies;
    var result = new PageList<CompanyDto>();

    if (companies != null)
    {
      if (filters == null)
      {
        result = await companies
          .Select(c => c.ToDto())
          .OrderBy(c => c.Name)
          .Page(ct);
      }
      else
      {
        result = await dbContext.Companies
          .WhereIf(filters.Name != null, c => c.Name.Contains(filters.Name!))
          .WhereIf(filters.HasActiveJobs != null, c => c.TotalJobsAdvertised > 0)
          .WhereIf(filters.Address != null, c => SearchByAddress(filters.Address!, c))
          .Select(c => c.ToDto())
          .OrderBy(c => c.Name)
          .Page(ct);
      }
    }

    return TypedResults.Ok(result);
  }

  private static bool SearchByAddress(AddressDto filter, Company company)
  {
    return company.Address.PostalCode.Contains(filter.PostalCode) || company.Address.Street.Contains(filter.Street) ||
           company.Address.Country.Contains(filter.Country) || company.Address.City.Contains(filter.City);
  }

  public static async Task<CreatedAtRoute> CreateCompany([FromBody] CompanyDto companyDto, JobVacancyDbContext dbContext)
  {
    var dto = companyDto.ToEntity();
    var entry = dbContext.Add(dto);
    await dbContext.SaveChangesAsync();

    return TypedResults.CreatedAtRoute(
      routeName: "GetById",
      routeValues: entry.Entity.Id
    );
  }

  public static async Task<Results<NoContent, NotFound>> UpdateCompanyById(Guid id, [FromBody] JsonPatchDocument<CompanyUpdateDto> patch, JobVacancyDbContext dbContext)
  {
    var entity = await dbContext.FindAsync<Company>(id);
    if (entity is null) return TypedResults.NotFound();

    var entityUpdateDto = entity.ToUpdateDto();
    patch.ApplyTo(entityUpdateDto);


    var updatedEntity = entityUpdateDto.ToEntity(entity.Id);
    dbContext.Update(updatedEntity);

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

  public static async Task<Results<NoContent, NotFound>> DeleteById(Guid companyId, JobVacancyDbContext dbContext, ILogger<V2CompanyEndpoints> logger)
  {
    var entity = await dbContext.FindAsync<Company>(companyId);

    if (entity is null) return TypedResults.NotFound();

    dbContext.Remove(entity);
    await dbContext.SaveChangesAsync();

    return TypedResults.NoContent();
  }

}

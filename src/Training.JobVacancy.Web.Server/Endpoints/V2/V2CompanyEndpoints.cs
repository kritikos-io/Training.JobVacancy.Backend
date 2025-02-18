namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Backend.Helpers;
using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class V2CompanyEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("company").WithTags("Company");

    group.MapPost("/search", Search).WithName("Search");
    group.MapGet("/{id:guid}", GetById).WithName("GetById");
    group.MapPost("", CreateCompany).WithName("CreateCompany");
    group.MapPut("/{id:guid}", UpdateCompanyById).WithName("UpdateCompanyById");
    group.MapDelete("/{id:guid}", DeleteById).WithName("DeleteById");

    return endpoint;
  }

  public static async Task<Ok<PageList<CompanyShortResponseDto>>> Search([FromBody] CompanyFilters? filters, JobVacancyDbContext dbContext,CancellationToken ct)
  {

    var result = await dbContext.Companies
      .WhereIf(!string.IsNullOrWhiteSpace(filters?.Name), c => c.Name.Contains(filters!.Name!))
      .WhereIf(!string.IsNullOrWhiteSpace(filters?.Vat), c => c.Vat.Contains(filters!.Vat!))
      .WhereIf(!string.IsNullOrWhiteSpace(filters?.PhoneNumber), c => c.PhoneNumber != null && c.PhoneNumber.Contains(filters!.PhoneNumber!))
      .WhereIf(filters?.Address != null, company => (company.Address.PostalCode!= null && company.Address.PostalCode.Contains(filters!.Address!.PostalCode!))
                                                    || (company.Address.Country!= null && company.Address.Country.Contains(filters!.Address!.Country!))
                                                    || (company.Address.Street!= null && company.Address.Street.Contains(filters!.Address!.Street!))
                                                    || (company.Address.City!= null && company.Address.City.Contains(filters!.Address!.City!))
                                                    || (company.Address.StreetNumber!= null && company.Address.StreetNumber.Contains(filters!.Address!.StreetNumber!)))
      .Select(c => c.ToShortResponseDto())
      .OrderBy(c => c.Name)
      .Page(ct);

    return TypedResults.Ok(result);
  }

  public static async Task<CreatedAtRoute<CompanyResponseDto>> CreateCompany([FromBody] CompanyRequestCreateDto dto, JobVacancyDbContext dbContext, ILogger<V2CompanyEndpoints> logger)
  {
    var entity = dto.ToEntity();

    try
    {
      await dbContext.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      logger.LogError($"Failed to create company {dto.Name}");
    }

    return TypedResults.CreatedAtRoute(
      routeName: "GetById",
      routeValues: entity.Id,
      value: entity.ToResponseDto()
    );
  }

  public static async Task<Results<Ok<CompanyResponseDto>, NotFound>> UpdateCompanyById(Guid id, CompanyRequestUpdateDto dto, JobVacancyDbContext dbContext,  ILogger<V2CompanyEndpoints> logger)
  {
    var entity = await dbContext.FindAsync<Company>(id);

    if (entity is null)
    {
      logger.LogEntityNotFound(nameof(Company), id);
      return TypedResults.NotFound();
    }

    entity.Apply(dto);

    try
    {
      await dbContext.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      logger.LogError($"Failed to create company {dto.Name}");
    }
    return TypedResults.Ok(entity.ToResponseDto());
  }

  public static async Task<Results<Ok<CompanyResponseDto>, NotFound>> GetById(Guid companyId, JobVacancyDbContext dbContext, ILogger<V2CompanyEndpoints> logger)
  {
    var entity = await dbContext.FindAsync<Company>(typeof(Company),companyId);

    if (entity is null)
    {
      logger.LogEntityNotFound(nameof(Company), companyId);
      return TypedResults.NotFound();
    }

    var dto = entity.ToResponseDto();
    return TypedResults.Ok(dto);
  }

  public static async Task<Results<NoContent, NotFound>> DeleteById(Guid companyId, JobVacancyDbContext dbContext, ILogger<V2CompanyEndpoints> logger)
  {
    var entity = await dbContext.FindAsync<Company>(companyId);

    if (entity is null)
    {
      logger.LogEntityNotFound(nameof(Company), companyId);
      return TypedResults.NotFound();
    }

    dbContext.Remove(entity);
    try
    {
      await dbContext.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      logger.LogError($"Failed to delete company {entity.Name}");
    }

    return TypedResults.NoContent();
  }

}

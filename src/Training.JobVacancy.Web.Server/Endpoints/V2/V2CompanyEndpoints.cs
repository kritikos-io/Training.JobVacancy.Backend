namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Adaptit.Training.JobVacancy.Data;
using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Server.Extensions;
using Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using LogTemplates = Adaptit.Training.JobVacancy.Web.Server.Helpers.LogTemplates;

public class V2CompanyEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("company").WithTags("Company");

    group.MapPost("search", Search).WithName("Search");
    group.MapGet("{companyId:guid}", GetById).WithName("GetById");
    group.MapPost("", CreateCompany).WithName("CreateCompany");
    group.MapPut("{companyId:guid}", UpdateCompanyById).WithName("UpdateCompanyById");
    group.MapDelete("{companyId:guid}", DeleteById).WithName("DeleteById");

    return endpoint;
  }

  public static async Task<Ok<PagedList<CompanyShortResponseDto>>> Search([FromBody] CompanyFilters? filters, JobVacancyDbContext dbContext, CancellationToken ct)
  {
    var result = await dbContext.Companies
        .WhereIf(!string.IsNullOrWhiteSpace(filters?.Name), c => c.Name.Contains(filters!.Name!))
        .WhereIf(!string.IsNullOrWhiteSpace(filters?.Vat), c => c.Vat.Contains(filters!.Vat!))
        .WhereIf(!string.IsNullOrWhiteSpace(filters?.PhoneNumber), c => c.PhoneNumber != null && c.PhoneNumber.Contains(filters!.PhoneNumber!))
        .WhereIf(filters?.Address != null,
            company => company.Address.PostalCode!.Contains(filters!.Address!.PostalCode!)
                       || company.Address.Country!.Contains(filters.Address.Country!)
                       || company.Address.Street!.Contains(filters.Address.Street!)
                       || company.Address.City!.Contains(filters.Address.City!)
                       || company.Address.StreetNumber!.Contains(filters.Address.StreetNumber!))
        .OrderBy(c => c.Name)
        .ToPagedListAsync(c => c.ToShortResponseDto(), pageSize: filters!.PageSize, pageNumber: filters!.PageNumber, cancellationToken: ct);

    return TypedResults.Ok(result);
  }

  public static async Task<Results<CreatedAtRoute<CompanyResponseDto>, Conflict>> CreateCompany(
      [FromBody] CompanyRequestCreateDto dto,
      JobVacancyDbContext dbContext,
      ILogger<V2CompanyEndpoints> logger,
      CancellationToken ct)
  {
    var entity = dto.ToEntity();
    dbContext.Add(entity);

    try
    {
      await dbContext.SaveChangesAsync(ct);
    }
    catch (DbUpdateException)
    {
      LogTemplates.LogEntityNotCreated(logger, nameof(Company), dto.Name);
      return TypedResults.Conflict();
    }

    return TypedResults.CreatedAtRoute(
        entity.ToResponseDto(),
        nameof(GetById),
        new { companyId = entity.Id });
  }

  public static async Task<Results<Ok<CompanyResponseDto>, NotFound, Conflict>> UpdateCompanyById(
      Guid companyId,
      CompanyRequestUpdateDto dto,
      JobVacancyDbContext dbContext,
      ILogger<V2CompanyEndpoints> logger,
      CancellationToken ct)
  {
    var entity = await dbContext.FindAsync<Company>([companyId], cancellationToken: ct);

    if (entity is null)
    {
      LogTemplates.LogEntityNotFound(logger, nameof(Company), companyId);
      return TypedResults.NotFound();
    }

    entity.UpdateEntity(dto);

    try
    {
      await dbContext.SaveChangesAsync(ct);
    }
    catch (DbUpdateException)
    {
      LogTemplates.LogEntityNotUpdated(logger, nameof(Company), companyId);
      return TypedResults.Conflict();
    }

    return TypedResults.Ok(entity.ToResponseDto());
  }

  public static async Task<Results<Ok<CompanyResponseDto>, NotFound>> GetById(
      Guid companyId,
      JobVacancyDbContext dbContext,
      ILogger<V2CompanyEndpoints> logger,
      CancellationToken ct)
  {

    var dto = await dbContext.Companies.Where(c => c.Id == companyId).Select(c => c.ToResponseDto()).FirstOrDefaultAsync(cancellationToken: ct);

    if (dto is null)
    {
      LogTemplates.LogEntityNotFound(logger, nameof(Company), companyId);
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(dto);
  }

  public static async Task<Results<NoContent, NotFound, Conflict>> DeleteById(
      Guid companyId,
      JobVacancyDbContext dbContext,
      ILogger<V2CompanyEndpoints> logger,
      CancellationToken ct)
  {
    var entity = await dbContext.FindAsync<Company>([companyId], cancellationToken: ct);

    if (entity is null)
    {
      LogTemplates.LogEntityNotFound(logger, nameof(Company), companyId);
      return TypedResults.NotFound();
    }

    dbContext.Remove(entity);

    try
    {
      await dbContext.SaveChangesAsync(ct);
    }
    catch (DbUpdateException)
    {
      LogTemplates.LogEntityNotDeleted(logger, nameof(Company), companyId);
      return TypedResults.Conflict();
    }

    return TypedResults.NoContent();
  }
}

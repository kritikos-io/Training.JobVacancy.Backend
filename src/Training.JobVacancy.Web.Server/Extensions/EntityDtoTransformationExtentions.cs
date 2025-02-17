namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

public static class EntityDtoTransformationExtentions
{
  public static CompanyDto ToDto(this Company entity)
  {
    return new CompanyDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Website = entity.Website,
      Vat = entity.Vat,
      LogoUrl = entity.LogoUrl,
      Address = ToDto(entity.Address),
      Sponsored = entity.Sponsored,
      TotalJobsAdvertised = entity.TotalJobsAdvertised,
      PhoneNumber = entity.PhoneNumber
    };
  }

  public static CompanyUpdateDto ToUpdateDto(this Company entity)
  {
    return new CompanyUpdateDto
    {
      Name = entity.Name,
      Website = entity.Website,
      Vat = entity.Vat,
      LogoUrl = entity.LogoUrl,
      Address = ToUpdateDto(entity.Address),
      Sponsored = entity.Sponsored,
      TotalJobsAdvertised = entity.TotalJobsAdvertised,
      PhoneNumber = entity.PhoneNumber
    };
  }

  public static Company ToEntity(this CompanyDto dto)
  {
    return new Company
    {
      Id = dto.Id,
      Name = dto.Name,
      Website = dto.Website,
      Vat = dto.Vat,
      LogoUrl = dto.LogoUrl,
      Address = ToEntity(dto.Address),
      Sponsored = dto.Sponsored,
      TotalJobsAdvertised = dto.TotalJobsAdvertised,
      PhoneNumber = dto.PhoneNumber
    };
  }

  public static Company ToEntity(this CompanyUpdateDto dto, Guid companyId)
  {
    var company = new Company();

    company.Id = companyId;

    if (!string.IsNullOrEmpty(dto.Name)) company.Name = dto.Name;

    if (dto.Website != null) company.Website = dto.Website;

    if (!string.IsNullOrEmpty(dto.Vat)) company.Vat = dto.Vat;

    if (dto.LogoUrl != null) company.LogoUrl = dto.LogoUrl;

    if (dto.Address != null) company.Address = ToEntity(dto.Address);

    if (dto.Sponsored != null) company.Sponsored = dto.Sponsored.Value;

    if (dto.TotalJobsAdvertised.HasValue) company.TotalJobsAdvertised = dto.TotalJobsAdvertised.Value;

    if (!string.IsNullOrEmpty(dto.PhoneNumber)) company.PhoneNumber = dto.PhoneNumber;

    return company;
  }

  public static AddressDto ToDto(this Address entity)
  {
    return new AddressDto
    {
      Street = entity.Street,
      City = entity.City,
      StreetNumber = entity.StreetNumber,
      PostalCode = entity.PostalCode,
      Country = entity.Country
    };
  }

  public static Address ToEntity(this AddressDto dto)
  {
    return new Address
    {
      Street = dto.Street,
      City = dto.City,
      StreetNumber = dto.StreetNumber,
      PostalCode = dto.PostalCode,
      Country = dto.Country
    };
  }

  public static AddressUpdateDto ToUpdateDto(this Address entity)
  {
    return new AddressUpdateDto()
    {
      Street = entity.Street,
      City = entity.City,
      StreetNumber = entity.StreetNumber,
      PostalCode = entity.PostalCode,
      Country = entity.Country
    };
  }

  public static Address ToEntity(this AddressUpdateDto dto)
  {
    return new Address
    {
      Street = dto.Street,
      City = dto.City,
      StreetNumber = dto.StreetNumber,
      PostalCode = dto.PostalCode,
      Country = dto.Country
    };
  }

}

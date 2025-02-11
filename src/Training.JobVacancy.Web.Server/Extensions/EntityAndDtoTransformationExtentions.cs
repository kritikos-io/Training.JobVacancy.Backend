namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;
using Adaptit.Training.JobVacancy.Web.Models.Entities;

public static class EntityAndDtoTransformationExtentions
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

  public static AddressDto ToDto(Address entity)
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

  public static Address ToEntity(AddressDto dto)
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

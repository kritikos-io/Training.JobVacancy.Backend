namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;

public static class EntityDtoTransformationExtentions
{
  public static CompanyResponseDto ToResponseDto(this Company entity)
  {
    return new CompanyResponseDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Website = entity.Website,
      Vat = entity.Vat,
      LogoUrl = entity.LogoUrl,
      Address = ToDto(entity.Address),
      Sponsored = entity.Sponsored,
      PhoneNumber = entity.PhoneNumber
    };
  }

  public static CompanyShortResponseDto ToShortResponseDto(this Company entity)
  {
    return new CompanyShortResponseDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Vat = entity.Vat,
      PhoneNumber = entity.PhoneNumber
    };
  }

  public static void Apply(this Company entity, CompanyRequestUpdateDto dto)
  {
    entity.Name = dto.Name;
    entity.Website = dto.Website;
    entity.Vat = dto.Vat;
    entity.LogoUrl = dto.LogoUrl;
    entity.Address = dto.Address?.ToEntity() ?? new Address();
    entity.Sponsored = dto.Sponsored;
    entity.PhoneNumber = dto.PhoneNumber;
  }

  public static Company ToEntity(this CompanyRequestCreateDto dto)
  {
    var c = new Company();

    c.Name = dto.Name;
    c.Website = dto.Website;
    c.Vat = dto.Vat;
    c.LogoUrl = dto.LogoUrl;
    c.Sponsored = dto.Sponsored;
    c.Address = dto.Address?.ToEntity() ?? new Address();
    c.PhoneNumber = dto.PhoneNumber;

    return c;
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

}

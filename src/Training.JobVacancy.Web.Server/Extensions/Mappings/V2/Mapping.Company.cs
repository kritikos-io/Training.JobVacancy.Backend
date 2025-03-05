namespace Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;

/// <summary>
/// Provides extension methods for transforming between Entity and DTO objects.
/// </summary>
public static partial class Mapping
{
  /// <summary>
  /// Converts a <see cref="Company"/> entity to a <see cref="CompanyResponseDto"/>.
  /// </summary>
  /// <param name="entity">The company entity to be converted.</param>
  /// <returns>A DTO containing detailed company information.</returns>
  public static CompanyResponseDto ToResponseDto(this Company entity)
  {

    var test = new List<JobAdDto>(entity.JobAds.Select(x => x.ToDto())).AsReadOnly();

    return new CompanyResponseDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Website = entity.Website,
      Vat = entity.Vat,
      LogoUrl = entity.LogoUrl,
      Address = ToDto(entity.Address),
      Sponsored = entity.Sponsored,
      PhoneNumber = entity.PhoneNumber,
      JobAds = new List<JobAdDto>(entity.JobAds.Select(x=>x.ToDto())).AsReadOnly(),
    };
  }

  /// <summary>
  /// Converts a <see cref="Company"/> entity to a short version DTO (<see cref="CompanyShortResponseDto"/>).
  /// This is useful when displaying a list of companies with limited details.
  /// </summary>
  /// <param name="entity">The company entity to be converted.</param>
  /// <returns>A DTO containing essential company details.</returns>
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

  /// <summary>
  /// Updates an existing <see cref="Company"/> entity with values from a <see cref="CompanyRequestUpdateDto"/>.
  /// </summary>
  /// <param name="entity">The company entity to be updated.</param>
  /// <param name="dto">The DTO containing the new values.</param>
  public static void UpdateEntity(this Company entity, CompanyRequestUpdateDto dto)
  {
    entity.Name = dto.Name;
    entity.Website = dto.Website;
    entity.Vat = dto.Vat;
    entity.LogoUrl = dto.LogoUrl;
    entity.Address = entity.Address?.UpdateEntity(dto.Address) ?? new Address();
    entity.Sponsored = dto.Sponsored;
    entity.PhoneNumber = dto.PhoneNumber;
  }

  public static Address? UpdateEntity(this Address? entity, AddressDto? dto)
  {
    if (entity is null)
    {
      return null;
    }

    entity.Country = dto?.Country;
    entity.City = dto?.City;
    entity.Street = dto?.Street;
    entity.StreetNumber = dto?.StreetNumber;
    entity.PostalCode = dto?.PostalCode;

    return entity;
  }

  /// <summary>
  /// Converts a <see cref="CompanyRequestCreateDto"/> into a <see cref="Company"/> entity.
  /// </summary>
  /// <param name="dto">The DTO containing company details.</param>
  /// <returns>A new instance of <see cref="Company"/> populated with data from the DTO.</returns>
  public static Company ToEntity(this CompanyRequestCreateDto dto)
  {
    var c = new Company
    {
      Name = dto.Name,
      Website = dto.Website,
      Vat = dto.Vat,
      LogoUrl = dto.LogoUrl,
      Sponsored = dto.Sponsored,
      Address = dto.Address?.ToEntity() ?? new Address(),
      PhoneNumber = dto.PhoneNumber
    };

    return c;
  }

  /// <summary>
  /// Converts an <see cref="Address"/> entity into an <see cref="AddressDto"/>.
  /// </summary>
  /// <param name="entity">The address entity to be converted.</param>
  /// <returns>A DTO containing address details.</returns>
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

  /// <summary>
  /// Converts an <see cref="AddressDto"/> into an <see cref="Address"/> entity.
  /// </summary>
  /// <param name="dto">The DTO containing address details.</param>
  /// <returns>A new instance of <see cref="Address"/> populated with data from the DTO.</returns>
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

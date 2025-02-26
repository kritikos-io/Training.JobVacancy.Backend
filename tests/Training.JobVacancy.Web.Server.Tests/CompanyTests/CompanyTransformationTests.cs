namespace Adaptit.Training.JobVacancy.Web.Server.Tests.CompanyTests;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.Company;
using Adaptit.Training.JobVacancy.Web.Models.Dto.V2.JobAd;
using Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;

using Shouldly;

public class CompanyTransformationTests
{
  [Theory]
  [MemberData(nameof(GetCompanyTestData))]
  public void Company_To_Dto_ReturnsCompanyDto(Company entity, CompanyResponseDto expectedResponseDto)
  {
    var dto = entity.ToResponseDto();

    dto.ShouldBeEquivalentTo(expectedResponseDto);
  }

  public static IEnumerable<object[]> GetCompanyTestData()
  {
    yield return
    [
    new Company
      {
        Id = Guid.Parse("c3b510a2-40bd-4b77-85bf-1c871538efb8"),
        Name = "Tech Corp",
        Website = new Uri("https://www.techcorp.com"),
        Vat = "123456789",
        LogoUrl = new Uri("https://www.techcorp.com/logo.png"),
        JobAds = new List<JobAd>().AsReadOnly(),
        Address = new Address
        {
          Country = "USA",
          City = "New York",
          Street = "5th Avenue",
          StreetNumber = "123",
          PostalCode = "10001"
        },
        Sponsored = true,
        PhoneNumber = "+1-555-123-4567"
      },
      new CompanyResponseDto
      {
        Id = Guid.Parse("c3b510a2-40bd-4b77-85bf-1c871538efb8"),
        Name = "Tech Corp",
        Website = new Uri("https://www.techcorp.com"),
        Vat = "123456789",
        LogoUrl = new Uri("https://www.techcorp.com/logo.png"),
        JobAds = new List<JobAdDto>().AsReadOnly(),
        Address = new AddressDto
        {
          Country = "USA",
          City = "New York",
          Street = "5th Avenue",
          StreetNumber = "123",
          PostalCode = "10001"
        },
        Sponsored = true,
        PhoneNumber = "+1-555-123-4567"
      }
    ];

    // Edge Case: Minimal Data
    yield return
    [
    new Company
      {
        Id = Guid.Empty,
        Name = string.Empty,
        Website = null,
        Vat = string.Empty,
        LogoUrl = null,
        JobAds = new List<JobAd>().AsReadOnly(),
        Address = new Address
        {
          Country = string.Empty,
          City = string.Empty,
          Street = string.Empty,
          StreetNumber = string.Empty,
          PostalCode = string.Empty
        },
        Sponsored = false,
        PhoneNumber = string.Empty
      },
      new CompanyResponseDto
      {
        Id = Guid.Empty,
        Name = string.Empty,
        Website = null,
        Vat = string.Empty,
        LogoUrl = null,
        JobAds = new List<JobAdDto>().AsReadOnly(),
        Address = new AddressDto
        {
          Country = string.Empty,
          City = string.Empty,
          Street = string.Empty,
          StreetNumber = string.Empty,
          PostalCode = string.Empty
        },
        Sponsored = false,
        PhoneNumber = string.Empty
      }
    ];

    // Edge Case: Missing Optional Fields
    yield return
    [
    new Company
      {
        Id = Guid.Parse("c410fdcb-c276-4ac0-87fa-16ccc29825b1"),
        Name = "No Logo Inc.",
        Website = null,
        Vat = "987654321",
        LogoUrl = null,
        JobAds = new List<JobAd>().AsReadOnly(),
        Address = new Address
        {
          Country = "Canada",
          City = "Toronto",
          Street = "Maple Street",
          StreetNumber = "456",
          PostalCode = "M5H 2N2"
        },
        Sponsored = false,
        PhoneNumber = "+1-555-987-6543"
      },
      new CompanyResponseDto
      {
        Id = Guid.Parse("c410fdcb-c276-4ac0-87fa-16ccc29825b1"),
        Name = "No Logo Inc.",
        Website = null,
        Vat = "987654321",
        LogoUrl = null,
        JobAds = new List<JobAdDto>().AsReadOnly(),
        Address = new AddressDto
        {
          Country = "Canada",
          City = "Toronto",
          Street = "Maple Street",
          StreetNumber = "456",
          PostalCode = "M5H 2N2"
        },
        Sponsored = false,
        PhoneNumber = "+1-555-987-6543"
      }
    ];
  }

}

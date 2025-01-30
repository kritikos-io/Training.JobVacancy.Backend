namespace Adaptit.Training.JobVacancy.Web.Server.Repositories;

using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using Bogus;
using Bogus.Extensions.UnitedKingdom;

public class NavJobVacancyRepo
{
  public NavJobVacancyRepo()
      : this(Random.Shared)
  {
  }

  public NavJobVacancyRepo(int seed)
      : this(new Random(seed))
  {
  }

  public NavJobVacancyRepo(Random random)
  {
    Randomizer.Seed = random;
    var contactFaker = new Faker<ContactDto>()
        .RuleFor(x => x.Name, f => f.Name.FullName())
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
        .RuleFor(x => x.Title, f => f.Name.JobTitle());
    Contacts = contactFaker.GenerateBetween(15, 40);

    var employerFaker = new Faker<EmployerDto>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Description, f => f.Company.Bs())
        .RuleFor(x => x.Organizationnumber, f => f.Finance.VatNumber(VatRegistrationNumberType.Standard))
        .RuleFor(x => x.HomePage, f => new Uri(f.Internet.UrlWithPath("https")));
    Employers = employerFaker.GenerateBetween(15, 150);

    var workLocationFaker = new Faker<WorkLocationDto>()
        .RuleFor(x => x.Country, f => f.Address.Country())
        .RuleFor(x => x.County, f => f.Address.County())
        .RuleFor(x => x.City, f => f.Address.City())
        .RuleFor(x => x.Municipal, f => f.Address.State())
        .RuleFor(x => x.Address, f => f.Address.StreetName())
        .RuleFor(x => x.PostalCode, f => f.Address.ZipCode());
    WorkLocations = workLocationFaker.GenerateBetween(10, 350);

    var jobAdDtoFaker = new Faker<JobAdDto>()
            .RuleFor(x => x.Contacts, f => f.PickRandom(Contacts, f.Random.Number(1, 5)).ToArray())
            .RuleFor(x => x.Employer, f => f.PickRandom(Employers, 1).First())
            .RuleFor(x => x.Locations, f => f.PickRandom(WorkLocations, f.Random.Number(1, 3)).ToArray())
            .RuleFor(x => x.Title, f => f.Name.JobTitle())
            .RuleFor(x => x.Description, f => f.Name.JobDescriptor())
            .RuleFor(x => x.Source, f => f.PickRandom<Source>(Source.ImportApi, Source.Stillingsregistrering))
            .RuleFor(x => x.Source, f => f.PickRandom<Source>(Source.Stillingsregistrering, Source.ImportApi))
            .RuleFor(x => x.ExpiresAt, f => f.Date.Future())
            .RuleFor(x => x.UpdatedAt, f => f.Date.Past())
            .RuleFor(x => x.PublishedAt, f => f.Date.Past())
            .RuleFor(x => x.ApplicationDue, f => f.Date.Future())
            .RuleFor(x => x.ApplicationUrl, f => new Uri(f.Internet.UrlWithPath("https", "nav.no")))
        ;
    JobAds = jobAdDtoFaker.GenerateBetween(10, 150);

    var entryDtoFaker = new Faker<EntryDto>()
        .RuleFor(x => x.Uuid, f => f.Random.Guid())
        .RuleFor(x => x.Status, f => f.PickRandom<EntryStatus>(EntryStatus.Active, EntryStatus.Inactive))
        .RuleFor(x => x.ModifiedAt, f => f.Date.Past())
        .RuleFor(x => x.JobAd, f => f.PickRandom(JobAds, 1).First());
    Entries = entryDtoFaker.GenerateBetween(25, 350);

    foreach (var entry in Entries)
    {
      entry.JobAd.Uuid = entry.Uuid;
    }

    Faker<FeedDto> feedFaker = new Faker<FeedDto>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Title, f => f.WaffleTitle())
        .RuleFor(x => x.Description, f => f.WaffleText())
        .RuleFor(x => x.HomePageUrl, f => new Uri(f.Internet.UrlWithPath("https", "nav.no")));

    var totalFeeds = Random.Shared.Next(2, 8);
    Feeds = feedFaker.Generate(totalFeeds);
    var entriesPerFeed = Entries
        .OrderBy(x => x.ModifiedAt)
        .Select((s, i) => new { s, i })
        .GroupBy(x => x.i % totalFeeds)
        .Select(g => g.Select(x => x.s).ToList())
        .ToList();

    for (var i = 0; i < totalFeeds; i++)
    {

      var feed = Feeds.ElementAt(i);
      feed.Items = entriesPerFeed[i]
          .Select(x => new VacancyDto
          {
            Id = x.Uuid,
            Title = x.JobAd.Title,
            Content = x.JobAd.Description,
            ModifiedAt = x.JobAd.UpdatedAt,
            EntryDetails = new EntryDetailsDto()
            {
              BusinessName = x.JobAd.Employer.Name,
              Municipal = x.JobAd.Locations.First()?.Municipal ?? string.Empty,
              Status = EntryStatus.Active,
              Uuid = x.Uuid,
              Title = x.JobAd.Title,
            }
          })
          .ToArray();

      feed.NextId = i < Feeds.Count - 1
          ? Feeds.ElementAt(i + 1).Id
          : Guid.Empty;
    }

  }

  public IReadOnlyCollection<ContactDto> Contacts { get; }
  public IReadOnlyCollection<EmployerDto> Employers { get; }

  public IReadOnlyCollection<WorkLocationDto> WorkLocations { get; }
  public IReadOnlyCollection<EntryDto> Entries { get; }
  public IReadOnlyCollection<JobAdDto> JobAds { get; }

  public IReadOnlyCollection<FeedDto> Feeds { get; }
  public IReadOnlyCollection<EntryDetailsDto> EntryDetails { get; }
}

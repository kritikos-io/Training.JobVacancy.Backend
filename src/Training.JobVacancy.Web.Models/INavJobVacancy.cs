namespace Adaptit.Training.JobVacancy.Web.Models;

using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using Refit;

/// <summary>
/// This API provides a feed of job vacancies in Norway. NAV’s Job Database contains a majority of all publicly advertised job vacancies in Norway.
/// The job vacancies are either directly registered at NAV or obtained through third parties/ATS systems.
/// Each change to an ad will generate a new entry in the feed, but when querying for the details of a vacancy you will only receive the newest data.
/// </summary>
/// <remarks>
/// If an ad is actively stopped (i.e. not simply inactive because of expiry), it will be updated by masking or removing certain fields
/// including title, employer, business and contact information.
///
/// Additionally, vacancies from Finn.no are not included in the API feed.
/// </remarks>
[Headers("Authorization: Bearer")]
public partial interface INavJobVacancy
{
  /// <inheritdoc cref="GetLatestFeedPageAsync(DateTimeOffset?,string?,System.Threading.CancellationToken)"/>
  [Get("/api/v1/feed")]
  internal Task<ApiResponse<FeedDto>> GetLatestFeedPageAsync([Header("If-Modified-Since")] string? modifiedSince, string? last, CancellationToken cancellationToken);

  /// <summary>
  /// Gets a feed page modified after <paramref name="modifiedSince"/>.
  /// </summary>
  /// <param name="modifiedSince">The earliest date to include in results.</param>
  /// <param name="last">Pass a non <see langword="null"/> value to get the most recent page instead of the earliest possible one.</param>
  /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A <see cref="FeedDto"/> modified after <paramref name="modifiedSince"/>. Passing a non null <paramref name="last"/> gets the most recent page.</returns>
  /// <remarks>Ads are never active for more than 6 months, according to API author there is never a point in asking ads older than that via <paramref name="modifiedSince"/>.</remarks>
  Task<ApiResponse<FeedDto>> GetLatestFeedPageAsync(DateTimeOffset? modifiedSince = null, bool? last = null, CancellationToken cancellationToken = default)
    => GetLatestFeedPageAsync(modifiedSince?.ToString("R"),
        last ?? false
            ? "last"
            : null,
        cancellationToken);

  /// <summary>
  /// Gets the specified feed page.
  /// </summary>
  /// <param name="feedPageId">The id of the feed page to fetch.</param>
  /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The requested <see cref="FeedDto"/>.</returns>
  [Get("/api/v1/feed/{feedPageId}")]
  Task<ApiResponse<FeedDto>> GetFeedPageAsync(Guid feedPageId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets details for a specified vacancy.
  /// </summary>
  /// <param name="entryId">The job ad to retrieve.</param>
  /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>An <see cref="EntryDto"/> with the details of the requested job ad.</returns>
  /// <remarks>Redacted entries will contain a null <see cref="JobAdDto"/> item.</remarks>
  [Get("/api/v1/feedentry/{entryId}")]
  Task<ApiResponse<EntryDto>> GetFeedEntryAsync(Guid entryId, CancellationToken cancellationToken = default);

  /// <inheritdoc cref="GetTokenAsync(System.Threading.CancellationToken)"/>
  [Get("/api/publicToken")]
  [Headers("Authorization:")]
  internal Task<ApiResponse<string>> GetTokenResponseAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets a public token for the NAV Job Vacancy Feed.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A bearer token that can be used to authenticat to the NAV Job Vacancy Feed</returns>
  /// <remarks>This should only be used for experiments.</remarks>
  public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
  {
    var response = await GetTokenResponseAsync(CancellationToken.None);
    return response is not { IsSuccessStatusCode: true }
        ? string.Empty
        : response.Content?
              .Replace("Current public token for Nav Job Vacancy Feed:", string.Empty)
              .Trim()
          ?? string.Empty;
  }
}

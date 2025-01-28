namespace Adaptit.Training.JobVacancy.Web.Models;

using Refit;

public interface INavJobVacancySandboxToken
{
  [Get("/api/publicToken")]
  internal Task<ApiResponse<string>> GetTokenResponseAsync(CancellationToken cancellationToken = default);

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

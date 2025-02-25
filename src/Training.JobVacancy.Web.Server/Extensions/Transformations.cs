namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using System.Security.Claims;

public static class Transformations
{
  public static Guid? GetUserIdFromClaims(this IEnumerable<Claim> claims)
  {
    var claim = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    return Guid.TryParse(claim, out var id)
        ? id
        : null;
  }
}

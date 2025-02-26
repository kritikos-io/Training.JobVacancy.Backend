namespace Adaptit.Training.JobVacancy.Web.Server.Extensions.Mappings.V2;

using System.Security.Claims;

using Adaptit.Training.JobVacancy.Data.Entities;

public static partial class Mapping
{
  public static User MapToUser(this List<Claim> claims)
  {
    var user = new User()
    {
      Id = claims.GetUserIdFromClaims()!.Value,
      Email = claims.FirstOrDefault(c => c.Type == "email")?.Value,
      Name = claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
      LastName = claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
    };

    return user;
  }
}

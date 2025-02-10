namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V1;

using Adaptit.Training.JobVacancy.Web.Models.Dto.NavJobVacancy;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class V1CompanyEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("company")
      .WithTags("Company");

    group.MapPost("", CreateCompany);

    return endpoint;
  }

  public static Results<Created, NotFound, ValidationProblem> CreateCompany([FromBody] CompanyDto company)
  {

    throw new NotImplementedException();
    //return TypedResults.CreatedAtRoute();
  }
}

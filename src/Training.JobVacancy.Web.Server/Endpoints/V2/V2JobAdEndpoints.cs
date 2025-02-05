namespace Adaptit.Training.JobVacancy.Web.Server.Endpoints.V2;

using Microsoft.AspNetCore.Http.HttpResults;

public class V2JobAdEndpoints
{
  public static RouteGroupBuilder Map(RouteGroupBuilder endpoint)
  {
    var group = endpoint.MapGroup("jobad")
          .WithTags("JobAd");

    group.MapGet("", GetAllJobAds);
    group.MapGet("{id:guid}", GetJobAd);

    group.MapPost("", CreateJobAd);
    group.MapPatch("{id:guid}", UpdateJobAd);
    group.MapDelete("{id:guid}", DeleteJobAd);

    return endpoint;
  }

  public static Results<Ok, NotFound> GetAllJobAds() => throw new NotImplementedException();

  public static Results<Ok, NotFound> GetJobAd(Guid id) => throw new NotImplementedException();

  public static Results<Created<Object>, BadRequest<string>> CreateJobAd(Object ad) => throw new NotImplementedException();

  public static Results<Ok<string>, NotFound<string>> UpdateJobAd(Guid id) => throw new NotImplementedException();

  public static Results<Ok<string>, NotFound<string>> DeleteJobAd(Guid id) => throw new NotImplementedException();
}

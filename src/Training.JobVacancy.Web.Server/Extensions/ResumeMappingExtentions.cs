namespace Adaptit.Training.JobVacancy.Web.Server.Extensions;

using Adaptit.Training.JobVacancy.Data.Entities;
using Adaptit.Training.JobVacancy.Web.Models.Dto.Resume;

public static class ResumeMappingExtentions
{
  public static ResumeReturnDto ToResumeReturnDto(this Resume resume)
  {
    var dto = new ResumeReturnDto
    {
      Id = resume.Id,
      DownloadUrl = resume.DownloadUrl,
      IsInUse = resume.IsInUse,
      CreatedAt = resume.CreatedAt,
    };

    return dto;
  }

  public static List<ResumeReturnDto> ToResumeReturnDtos(this List<Resume> resumes)
  {
    var dtos = resumes.Select(r => r.ToResumeReturnDto()).ToList();

    return dtos;
  }
}

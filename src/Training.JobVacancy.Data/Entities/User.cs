#nullable enable
namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;
using System.Collections.Generic;

public class User
{
  public Guid Id { get; init; }
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  public List<Resume> Resumes { get; set; } = [];
}

#nullable enable
namespace Adaptit.Training.JobVacancy.Data.Entities;

using System;

public class User
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Surname { get; set; } = string.Empty;
  public Uri? Resume { get; set; }
}

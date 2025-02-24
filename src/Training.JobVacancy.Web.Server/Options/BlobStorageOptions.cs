namespace Adaptit.Training.JobVacancy.Web.Server.Options;

using System.ComponentModel.DataAnnotations;

public class BlobStorageOptions
{
  internal const string Section = "AzureBlobStorage";

  [Required]
  [Range(1,180, ErrorMessage = "ValidForMinutes must be between 1 and 180")]
  public int SasValidForMinutes { get; set; }

  [Required]
  public string ContainerName { get; set; } = "resumes";

  [Required]
  public string ConnectionString { get; set; } = string.Empty;

}

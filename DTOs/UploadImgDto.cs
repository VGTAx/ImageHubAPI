namespace ImageHubAPI.DTOs
{
  /// <summary>
  /// DTO for upload imange
  /// </summary>
  public class UploadImgDto
  {
    /// <summary>
    /// List of images
    /// </summary>
    public List<IFormFile>? Images { get; set; }
    /// <summary>
    /// User ID which upload images
    /// </summary>
    /// <example>55d8220f-2967-4342-8f6c-e6294a3e52c2</example>
    public string? UserID { get; set; }
  }
}

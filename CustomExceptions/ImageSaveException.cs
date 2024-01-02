namespace ImageHubAPI.CustomExceptions
{
  /// <summary>
  /// 
  /// </summary>
  public class ImageSaveException : Exception
  {
    /// <summary>
    /// 
    /// </summary>
    public ImageSaveException() : base() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public ImageSaveException(string message) : base(message) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ImageSaveException(string message, Exception innerException) : base(message, innerException) { }
  }
}

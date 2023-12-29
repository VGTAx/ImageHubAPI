namespace ImageHubAPI.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  public interface IDirectory
  {
    /// <summary>
    /// Determines whether the given path refers to an existing directory on disk.
    /// </summary>
    /// <param name="path">The path to test</param>
    /// <returns>true if path refers to an existing directory; false if the directory does not exist or an error occurs when trying to determine if the specified directory exists.</returns>
    bool Exists(string path);

    /// <summary>
    /// Creates all the directories in a specified path. 
    /// </summary>
    /// <param name="path">The directory to create</param>
    /// <returns>An object <see cref="DirectoryInfo"/> that represents the directory at the specified path.</returns>
    DirectoryInfo CreateDirectory(string path);
  }
}

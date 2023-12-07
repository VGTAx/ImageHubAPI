namespace ImageHubAPI.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  public interface IUserImgRepository<T> where T : class
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    Task<List<string>> GetImgByUserIdAsync(string userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    void UpdateUserWithImages(T user);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsUserExistAsync(string userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imgName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsImageAlreadyAddedAsync(string imgName, string userId);
  }
}

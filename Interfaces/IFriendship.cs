namespace ImageHubAPI.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  public interface IFriendship<T> where T : class
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <returns></returns>
    Task<T> GetFriendshipAsync(string userId, string friendId);
  }
}

using ImageHubAPI.DTOs;
using ImageHubAPI.Models;

namespace ImageHubAPI.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IUserFriendRepository<T> where T : class
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddFriend(T user);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<T> GetUserByIdAsync(string userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<T> GetUserByEmailAsync(string email);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsUserExistAsync(string userId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> IsFriendAddAsync(string userId, string friendId);
  }


}

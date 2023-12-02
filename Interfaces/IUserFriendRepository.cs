using ImageHubAPI.DTOs;
using ImageHubAPI.Models;

namespace ImageHubAPI.Interfaces
{
  public interface IUserFriendRepository<T> where T : class
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddFriend(User user);
    
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
    Task<T> GetUserByEmail(string email);

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

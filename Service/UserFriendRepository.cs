using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
  /// <summary>
  /// 
  /// </summary>
  public class UserFriendRepository : IUserFriendRepository<User>
  {
    private readonly ImageHubContext? _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public UserFriendRepository(ImageHubContext context)
    {
      _context = context;
    }

    /// <summary>
    /// 
    /// </summary>
    public UserFriendRepository() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task AddFriend(User user)
    {
      _context.Users.Update(user);
      await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<User> GetUserByIdAsync(string userId)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<User> GetUserByEmailAsync(string email) =>
      await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    /// <summary>
    /// Checks the existence of a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Result checking the existence of a user. True if user exists, else false</returns>
    public async Task<bool> IsUserExistAsync(string userId) =>
        await _context.Users.AnyAsync(u => u.Id == userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <returns></returns>
    public async Task<bool> IsFriendAddAsync(string userId, string friendId) =>
      await _context.Friendships.AnyAsync(fr => fr.UserSenderId == userId && fr.FriendId == friendId);

  }
}

using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
  /// <summary>
  /// 
  /// </summary>
  public class FriendshipRepository : IFriendshipRepository<Friendship>
  {
    private readonly ImageHubContext _context;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public FriendshipRepository(ImageHubContext context)
    {
      _context = context;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <returns></returns>
    public async Task<Friendship> GetFriendshipAsync(string userId, string friendId) =>
      await _context.Friendships.FirstOrDefaultAsync(fr => fr.FriendId == friendId && fr.UserSenderId == userId);
      

  }
}

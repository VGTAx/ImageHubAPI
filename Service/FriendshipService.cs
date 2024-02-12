using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class FriendshipService : IFriendship<Friendship>
    {
        private readonly IImageHubContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public FriendshipService(ImageHubContext context)
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

using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserFriendService : IUserFriend<User>
    {
        private readonly IImageHubContext? _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UserFriendService(IImageHubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserFriendService() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddFriendAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

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

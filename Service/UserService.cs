using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUser<User>
    {
        private readonly IImageHubContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UserService(IImageHubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Checks the existence of a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Result checking the existence of a user. True if user exists, else false</returns>
        public async Task<bool> IsUserExistAsync(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}

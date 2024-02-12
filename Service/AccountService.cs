using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
    public class AccountService : IAccount
    {
        private readonly IImageHubContext _context;

        public AccountService(ImageHubContext context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailAvailable(string email) =>
          await _context.Users.AnyAsync(u => u.Email == email);
    }
}

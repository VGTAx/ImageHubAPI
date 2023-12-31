using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
  /// <summary>
  /// 
  /// </summary>
  public class UserImgRepository : IUserImgRepository<User>
  {
    /// <summary>
    /// 
    /// </summary>
    private readonly ImageHubContext _context;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    public UserImgRepository(ImageHubContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetImgByUserIdAsync(string userId)
    {
      var images = await _context.Images
          .Where(i => i.UserId == userId)
          .Select(i => _configuration.GetSection("PathImgHub").Value + i.Path)
          .ToListAsync();

      return images;
    }

    /// <summary>
    /// Checks an image has been added or not
    /// </summary>
    /// <param name="imgName">Image name</param>
    /// <param name="userId">User ID</param>
    /// <returns>Returns the result of checking an image has been added or not. True if image has been added, else false</returns>
    public async Task<bool> IsImageAlreadyAddedAsync(string imgName, string userId) =>
        await _context.Images.AnyAsync(i => i.Title == imgName && i.UserId == userId);

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
    /// <returns></returns>
    public async Task SaveChangesAsync() =>
      await _context.SaveChangesAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SaveImageAsync(IFormFile formFile, string path)
    {
      using (FileStream fs = new FileStream(path, FileMode.Create))
      {
        await formFile.CopyToAsync(fs);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>    
    public void UpdateUserWithImages(User user) =>
      _context.Users.Update(user);
  }
}

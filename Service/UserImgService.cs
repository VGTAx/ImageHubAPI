using ImageHubAPI.CustomExceptions;
using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserImgService : IUserImg<User>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IImageHubContext _context;
        private readonly IConfiguration _configuration;
        private bool _isSuccessful;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public UserImgService(ImageHubContext context, IConfiguration configuration)
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
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveImageAsync(List<IFormFile> formFile, string path, User user)
        {
            try
            {
                foreach (var image in formFile)
                {
                    string fullpath = $"{path}/{image.FileName}";
                    using (FileStream fs = new FileStream(fullpath, FileMode.Create))
                    {
                        await image.CopyToAsync(fs);
                    }
                    _isSuccessful = true;

                    var img = new Image
                    {
                        ImageId = Guid.NewGuid().ToString(),
                        Title = image.FileName,
                        Path = $"/{user.Id}/{image.FileName}",
                        UserId = user.Id
                    };

                    user!.AddImages(img);
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _isSuccessful = false;
                throw new ImageSaveException("Failed to save image.", ex.InnerException!);
            }
        }

        /// <summary>
        /// Return upload path for user
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>Path or <see cref="null"/></returns>
        public string? GetUploadPath(string userId)
        {
            var path = $"{_configuration.GetSection("PathImgHub").Value}";
            return String.IsNullOrEmpty(path) ? null : $"{path}/{userId}";
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccessful => _isSuccessful;
    }
}

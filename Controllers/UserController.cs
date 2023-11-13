using ImageHubAPI.Data;
using ImageHubAPI.DTOs;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace ImageHubAPI.Controllers
{
    /// <summary>
    /// Controller to managing users
    /// </summary>
    [Authorize(Policy = "UserAccess")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IImageHubContext _context;

        /// <summary>
        /// UserController constructor
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        /// <param name="context">DB Context</param>
        public UserController(IConfiguration configuration, IImageHubContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Add friend
        /// </summary>
        /// <param name="addFriendDto" example='{"UserId":"55d8220f-2967-4342-8f6c-e6294a3e52c2", "FriendId":"23ad2a4f-c1f0-4abc-94c0-52854af2039e"}'>DTO for add friend</param>
        /// <response code="200">Friend added</response>
        /// <response code="400">
        ///   <ul>
        ///     <li>The request contains invalid data or invalid parameters</li>
        ///     <li>Friend was alreday added</li>
        ///   </ul>
        /// </response>
        /// <response code="401">User is not authorized</response>
        /// <response code="403">There are no permissions to do the operation</response>
        /// <response code="404">User not exist</response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="AddFriend"/> method</returns>
        [HttpPost(template: nameof(AddFriend), Name = nameof(AddFriend))]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendDto addFriendDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (!await IsUserExist(addFriendDto.UserId!))
                {
                    return NotFound($"User with ID:{addFriendDto.UserId} not exist");
                }

                if (!await IsUserExist(addFriendDto.FriendId!))
                {
                    return NotFound($"User with ID:{addFriendDto.FriendId} not exist");
                }

                if (!IsUserIdValid(addFriendDto.UserId!))
                {
                    return StatusCode(403, "There are no permissions to do the operation");
                }

                var IsFriendshipExist = await _context.Friendships.AnyAsync(fr => fr.UserSenderId == addFriendDto.UserId
                      && fr.FriendId == addFriendDto.FriendId);

                if (IsFriendshipExist)
                {
                    return BadRequest($"Friend with ID: {addFriendDto.FriendId} was already added");
                }

                var requester = await _context.Users.FirstOrDefaultAsync(u => u.Id == addFriendDto.UserId);

                var friendship = new Friendship
                {
                    UserSenderId = addFriendDto.UserId,
                    FriendId = addFriendDto.FriendId,
                    FriendshipId = Guid.NewGuid().ToString()
                };

                requester!.AddFriendship(friendship);
                
                _context.Users.Update(requester);
                await _context.SaveChangesAsync();

                return Ok("Friend added");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Upload images
        /// </summary>
        /// <response code="200">File(s) has(have) uploaded</response>
        /// <response code="400">
        ///   <ul>
        ///     <li>No images to download</li>
        ///     <li>The request contains invalid data or invalid parameters</li>
        ///     <li>Image has already added</li>
        ///   </ul>
        /// </response>
        /// <response code="401">User is not authorized</response>
        /// <response code="403">There are no permissions to do the operation</response>
        /// <response code="404">User not exist</response>
        /// <response code="500">Internal server error</response>
        /// <param name="uploadImgDto">DTO for upload images</param>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="UploadImg"/> method</returns>
        [HttpPost(template: nameof(UploadImg), Name = nameof(UploadImg))]
        public async Task<IActionResult> UploadImg([FromForm] UploadImgDto uploadImgDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (!await IsUserExist(uploadImgDto.UserID!))
                {
                    return NotFound($"User with ID:{uploadImgDto.UserID} not exist");
                }

                if (!IsUserIdValid(uploadImgDto.UserID!))
                {
                    return StatusCode(403, "There are no permissions to do the operation");
                }

                if (uploadImgDto.Images is null || uploadImgDto.Images.Count == 0)
                {
                    return BadRequest("No images to download");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == uploadImgDto.UserID);

                var images = uploadImgDto.Images;
                var uploadPath = $"{_configuration.GetSection("PathImgHub").Value}/{uploadImgDto.UserID}";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var img in images)
                {
                    string fullpath = $"{uploadPath}/{img.FileName}";

                    using (FileStream fs = new FileStream(fullpath, FileMode.Create))
                    {
                        await img.CopyToAsync(fs);
                    }

                    if (await IsImageAlreadyAdded(img.FileName, user.Id))
                    {
                        return BadRequest($"Image \"{img.FileName}\" has already added");
                    }

                    var image = new Image
                    {
                        ImageId = Guid.NewGuid().ToString(),
                        Title = img.FileName,
                        Path = $"/{uploadImgDto.UserID}/{img.FileName}",
                        UserId = uploadImgDto.UserID
                    };

                    user!.AddImages(image);

                    _context.Users.Update(user);
                }

                await _context.SaveChangesAsync();

                return Ok($"File(s) has(have) uploaded. Count:{uploadImgDto.Images.Count}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Get user images
        /// </summary>
        /// <param name="userId" example="55d8220f-2967-4342-8f6c-e6294a3e52c2">User ID</param>
        /// <response code="200">Images received</response>
        /// <response code="400">User ID is required</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="403">There are no permissions to do the operation</response>
        /// <response code="404">
        ///   <ul>
        ///     <li>User not exist</li>
        ///     <li>User doesn't have images</li>
        ///   </ul>
        /// </response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="GetUserImg"/> method</returns>
        [HttpGet(template: nameof(GetUserImg), Name = nameof(GetUserImg))]
        public async Task<IActionResult> GetUserImg([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is required!");
                }

                if (!await IsUserExist(userId!))
                {
                    return NotFound($"User with ID:{userId} not exist");
                }

                if (!IsUserIdValid(userId))
                {
                    return StatusCode(403, "There are no permissions to do the operation");
                }

                var images = _context.Images
                  .Where(i => i.UserId == userId)
                  .Select(i => _configuration.GetSection("PathImgHub").Value + i.Path);

                if (!images.Any())
                {
                    return NotFound("User doesn't have images");
                }

                return Ok(new { Images = images, Message = "Images received" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get friend images
        /// </summary>
        /// <param name="friendId" example="23ad2a4f-c1f0-4abc-94c0-52854af2039e">Friend ID</param>
        /// <response code="200">Images received</response>
        /// <response code="400">User ID is required</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="403">There are no permissions to do the operation</response>
        /// <response code="404">
        ///   <ul>
        ///     <li>User not exist</li>
        ///     <li>User doesn't have images</li>
        ///   </ul>
        /// </response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="GetFriendImg"/> method</returns>
        [HttpGet(template: nameof(GetFriendImg), Name = nameof(GetFriendImg))]
        public async Task<IActionResult> GetFriendImg([FromQuery] string friendId)
        {
            try
            {
                if (string.IsNullOrEmpty(friendId))
                {
                    return BadRequest($"Friend ID is required");
                }

                if (!await IsUserExist(friendId))
                {
                    return NotFound($"User with ID: {friendId} not exist");
                }

                var userId = User.FindFirst("UserID")!.Value;

                var availableFriendImg = await _context.Friendships.FirstOrDefaultAsync(fr => fr.FriendId == friendId);

                if (availableFriendImg == null || !IsUserIdValid(userId))
                {
                    return StatusCode(403, "There are no permissions to do the operation");
                }

                var images = _context.Images
                  .Where(i => i.UserId == friendId)
                  .Select(i => _configuration.GetSection("PathImgHub").Value + i.Path);

                if (!images.Any())
                {
                    return NotFound($"User with ID: {friendId} doesn't have images");
                }

                return Ok(images);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Return user by email
        /// </summary>
        /// <param name="email" example="username_2@example.com">User email</param>
        /// <response code="200">Images received</response>
        /// <response code="400">The request contains invalid data or invalid parameters</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="404">User not exist</response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="GetUser"/> method</returns>
        [HttpGet(template: nameof(GetUser), Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound($"User with Email: {email} not exist");
                }

                return Ok(new { UserId = user.Id, Username = user.Name, user.Email, Message = "User received" });
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Checks the existence of a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Result checking the existence of a user. True if user exists, else false</returns>
        private async Task<bool> IsUserExist(string userId) =>
            await _context.Users.AnyAsync(u => u.Id == userId);

        /// <summary>
        /// Checks an image has been added or not
        /// </summary>
        /// <param name="imgName">Image name</param>
        /// <param name="userId">User ID</param>
        /// <returns>Returns the result of checking an image has been added or not. True if image has been added, else false</returns>
        private async Task<bool> IsImageAlreadyAdded(string imgName, string userId) =>
            await _context.Images.AnyAsync(i => i.Title == imgName && i.UserId == userId);

        /// <summary>
        /// Checks the user id from claims and the current id. 
        /// </summary>
        /// <param name="userId">Current user ID</param>
        /// <returns></returns>
        private bool IsUserIdValid(string userId) => User.FindFirst("UserID")!.Value == userId;

    }
}

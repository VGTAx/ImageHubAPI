using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageHubAPI.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Authorize(Policy = "UserAccess")]
  [Route("api/[controller]")]
  [ApiController]
  public class UserImgController : ControllerBase
  {

    private readonly IConfiguration _configuration;
    private readonly IUserImgRepository<User> _userImgRepository;
    private readonly IUserFriendRepository<User> _userFriendRepository;
    private readonly IFriendshipRepository<Friendship> _friendshipRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userImgRepository"></param>
    /// <param name="userFriendRepository"></param>
    /// <param name="configuration"></param>
    /// <param name="friendshipRepository"></param>
    public UserImgController(
      IUserImgRepository<User> userImgRepository,
      IUserFriendRepository<User> userFriendRepository,
      IConfiguration configuration,
      IFriendshipRepository<Friendship> friendshipRepository)
    {
      _userImgRepository = userImgRepository;
      _userFriendRepository = userFriendRepository;
      _configuration = configuration;
      _friendshipRepository = friendshipRepository;
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

        if (!await _userImgRepository.IsUserExistAsync(uploadImgDto.UserID!))
        {
          return NotFound($"User with ID:{uploadImgDto.UserID} not exist");
        }

        if (!IsUserIdValid(uploadImgDto.UserID!))
        {
          return Forbid("There are no permissions to do the operation");
        }

        if (uploadImgDto.Images is null || uploadImgDto.Images.Count == 0)
        {
          return BadRequest("No images to download");
        }

        var user = await _userFriendRepository.GetUserByIdAsync(uploadImgDto.UserID!);

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

          if (!await _userImgRepository.IsImageAlreadyAddedAsync(img.FileName, user!.Id))
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

          _userImgRepository.UpdateUserWithImages(user);
        }

        await _userImgRepository.SaveChangesAsync();

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

        if (!await _userImgRepository.IsUserExistAsync(userId!))
        {
          return NotFound($"User with ID:{userId} not exist");
        }

        if (!IsUserIdValid(userId))
        {
          return Forbid("There are no permissions to do the operation");
        }

        var images = await _userImgRepository.GetImgByUserIdAsync(userId);

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

        if (!await _userImgRepository.IsUserExistAsync(friendId))
        {
          return NotFound($"User with ID: {friendId} not exist");
        }
        var userId = User?.FindFirst("UserID")?.Value;

        var availableFriendImg = await _friendshipRepository.GetFriendshipAsync(userId, friendId);

        if (availableFriendImg == null || !IsUserIdValid(userId))
        {
          return Forbid("There are no permissions to do the operation");
        }

        var images = await _userImgRepository.GetImgByUserIdAsync(friendId);

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
    /// Checks the user id from claims and the current id. 
    /// </summary>
    /// <param name="userId">Current user ID</param>
    /// <returns></returns>
    private bool IsUserIdValid(string userId) =>
      User?.FindFirst("UserID")?.Value == userId;
  }
}

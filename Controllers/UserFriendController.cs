using ImageHubAPI.Data;
using ImageHubAPI.DTOs;
using ImageHubAPI.Models;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ImageHubAPI.Controllers
{
  /// <summary>
  /// Controller to managing users
  /// </summary>
  [Authorize(Policy = "UserAccess")]
  [Route("api/[controller]")]
  [ApiController]
  public class UserFriendController : ControllerBase
  {    
    private readonly IUserFriendRepository<User> _repository;

    /// <summary>
    /// UserController constructor
    /// </summary>
    /// <param name="configuration">Application configuration</param>
    /// <param name="context">DB Context</param>
    public UserFriendController(IUserFriendRepository<User> repository)
    {      
      _repository = repository;
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
        
        if (!await _repository.IsUserExistAsync(addFriendDto.UserId!))
        {
          return NotFound($"User with ID:{addFriendDto.UserId} not exist");
        }

        if (!await _repository.IsUserExistAsync(addFriendDto.FriendId!))
        {
          return NotFound($"User with ID:{addFriendDto.FriendId} not exist");
        }

        if (!IsUserIdValid(addFriendDto.UserId!))
        {
          return Forbid("There are no permissions to do the operation");
        }        

        if (!await _repository.IsFriendAddAsync(addFriendDto.UserId!, addFriendDto.FriendId!))
        {
          return BadRequest($"Friend with ID: {addFriendDto.FriendId} was already added");
        }

        var requester = await _repository.GetUserByIdAsync(addFriendDto.UserId);

        if(requester == null) 
        {
          return NotFound($"User with ID:{addFriendDto.UserId} not exist");
        }

        var friendship = new Friendship
        {
          UserSenderId = addFriendDto.UserId,
          FriendId = addFriendDto.FriendId,
          FriendshipId = Guid.NewGuid().ToString()
        };

        requester!.AddFriendship(friendship);

        await _repository.AddFriend(requester);

        return Ok("Friend added");
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

        var user = await _repository.GetUserByEmail(email);

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
    /// Checks the user id from claims and the current id. 
    /// </summary>
    /// <param name="userId">Current user ID</param>
    /// <returns></returns>
    private bool IsUserIdValid(string userId) =>
      User?.FindFirst("UserID").Value == userId;    
  }
}

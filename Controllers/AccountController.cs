using ImageHubAPI.Data;
using ImageHubAPI.IService;
using ImageHubAPI.Models;
using ImageHubAPI.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace ImageHubAPI.Controllers
{
    /// <summary>
    /// Controller for managing account
    /// </summary>
    [ApiController]
    [Route("api/Account")]
    public class AccountController : ControllerBase
    {
        private readonly IImageHubContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore<User> _userStore;
        private readonly IJwtGenerator _jwtGenerator;

        /// <summary>
        /// AccountController constructor
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="userManager">User manager</param>
        /// <param name="signInManager">Sign-in manager</param>
        /// <param name="userStore">User storage</param>
        /// <param name="jwtGenerator">JWT token generator</param>
        public AccountController(
          IImageHubContext context,
          UserManager<User> userManager,
          SignInManager<User> signInManager,
          IUserStore<User> userStore,
          IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _jwtGenerator = jwtGenerator;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration" example='{"Email":"user@example.com", "name":"user", "password":"passworD1!", "confirmPassword":"passworD1!"}'>
        /// Data for registering a new user</param>
        /// <response code="200">User registered</response>
        /// <response code="400">
        ///   <ul>
        ///     <li>Email has already taken</li>
        ///     <li>The request contains invalid data or invalid parameters.</li>
        ///   </ul>
        /// </response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="Registration"/> method</returns>
        [HttpPost(template: nameof(Registration), Name = nameof(Registration))]
        public async Task<IActionResult> Registration([FromBody] Registration registration)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (await IsEmailAvailable(registration.Email!))
                {
                    return BadRequest($"Email \"{registration.Email}\" has already taken");
                }

                var user = new User
                {
                    Name = registration.Name,
                    UserName = registration.Email,
                    Email = registration.Email
                };

                await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, registration.Password!);

                if (result.Succeeded)
                {
                    return Ok("User register");
                }

                return BadRequest(result.Errors);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="login" example='{"email":"username_1@example.com", "password":"passworD1!", "rememberMe":true}'></param>
        /// <response code="200">User logged in</response>
        /// <response code="400">The request contains invalid data or invalid parameters</response>
        /// <response code="401">Login or password isn't correct</response>
        /// <response code="500">Internal server error</response>
        /// <returns>Returns an HTTP status code indicating the result of the <see cref="Login"/> method and access token</returns>
        [HttpPost(template: nameof(Login), Name = nameof(Login))]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _signInManager.PasswordSignInAsync(login.Email!, login.Password!, login.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(login.Email!);
                    var tokenString = _jwtGenerator.CreateToken(user!);
                    
                    return Ok(new { Token = tokenString, Message = "User logged in" });
                }

                return Unauthorized("login or password isn't correct");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Checks the availability of an email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Result checking the availability of an email.</returns>
        private async Task<bool> IsEmailAvailable(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);
    }
}

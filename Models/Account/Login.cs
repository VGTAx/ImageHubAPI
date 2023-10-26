using System.ComponentModel.DataAnnotations;

namespace ImageHubAPI.Models.Account
{
  /// <summary>
  /// Login model
  /// </summary>
  public class Login
  {
    /// <summary>
    /// Email
    /// </summary>
    /// <example>username_3@example.com</example>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    /// <summary>
    /// Password
    /// </summary>
    /// <example>Password!1</example>
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    /// <summary>
    /// Value indicating whether the application should "remember" the user after authentication.
    /// </summary>
    public bool RememberMe { get; set; }
  }
}

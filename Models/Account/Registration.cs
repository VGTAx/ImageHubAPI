using System.ComponentModel.DataAnnotations;

namespace ImageHubAPI.Models.Account
{
    /// <summary>
    /// Registration model
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <example>username_3@example.com</example>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <example>User</example>
        [Required]
        public string? Name { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <example>Password!1</example>
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
        /// <summary>
        /// Confirm password
        /// </summary>
        /// <example>Password!1</example>
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ImageHubAPI.Models
{
  /// <summary>
  /// <see cref="Friendship"/> is used to track connections between users who are friends.
  /// </summary>
  public class Friendship
  {
    /// <summary>
    /// Friendship Id
    /// </summary>
    [Key]
    public string? FriendshipId { get; set; }
    /// <summary>
    /// First user id
    /// </summary>
    public string? FirstUserId { get; set; }
    /// <summary>
    /// Second user id
    /// </summary>
    public string? SecondUserId { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public User? FirstUser { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public User? SecondUser { get; set; }
  }
}

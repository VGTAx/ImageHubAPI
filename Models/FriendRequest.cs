using ImageHubAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageHubAPI.Models
{
  /// <summary>
  /// <see cref="FriendRequest"/> is used for friendship requests between users.
  /// </summary>
  public class FriendRequest
  {
    /// <summary>
    /// Fried request ID
    /// </summary>
    [Key]
    public string? FriendRequestId { get; set; }
    /// <summary>
    /// Requester ID
    /// </summary>
    public string? RequesterId { get; set; }
    /// <summary>
    /// Receiver ID
    /// </summary>
    public string? ReceiverId { get; set; }
    /// <summary>
    /// Request status
    /// </summary>
    public FriendshipRequestStatus? Status { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public User? Requester { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public User? Receiver { get; set; }
  }
}

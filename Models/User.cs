using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ImageHubAPI.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class User : IdentityUser
  {
    /// <summary>
    /// User Id
    /// </summary>
    [Required]
    public string? UserId { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// List of images
    /// </summary>
    private readonly List<Image>? _userImages = new List<Image>();

    /// <summary>
    /// List of friendships
    /// </summary>
    private readonly List<Friendship> _friendships = new List<Friendship>();

    /// <summary>
    /// List of friendrequest
    /// </summary>
    private readonly List<FriendRequest> _friendRequests = new List<FriendRequest>();

    /// <summary>
    /// Provides access to a collection of <see cref="Image"/> that belong to a given user.
    /// </summary>
    public IReadOnlyCollection<Image> UserImages => _userImages!;

    /// <summary>
    /// Provides access to a collection of <see cref="Friendship"/> that are associated with a given user.
    /// </summary>
    public IReadOnlyCollection<Friendship> Friendships => _friendships!;

    /// <summary>
    /// Provides access to a collection of <see cref="FriendRequest"/> sent and received by a given user.
    /// </summary>
    public IReadOnlyCollection<FriendRequest> FriendRequests => _friendRequests!;

    /// <summary>
    /// Add image to user
    /// </summary>
    /// <param name="image">Image to add</param>
    public void AddImages(Image image) => _userImages!.Add(image);

    /// <summary>
    /// Add friendship beetween users
    /// </summary>
    /// <param name="friendship">Friendship to add</param>
    public void AddFriendship(Friendship friendship) => _friendships.Add(friendship);

    /// <summary>
    /// Add friend request
    /// </summary>
    /// <param name="friendRequest">Friend request</param>
    public void AddFriendRequest(FriendRequest friendRequest) => _friendRequests.Add(friendRequest);
    
  }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(Order = 1)]
        public string? FriendshipId { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        [Column(Order = 2)]
        public string? UserSenderId { get; set; }

        /// <summary>
        /// Friend id
        /// </summary>
        [Column(Order = 3)]
        public string? FriendId { get; set; }

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

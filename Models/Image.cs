using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ImageHubAPI.Models
{
    /// <summary>
    /// <see cref="Image"/> contains information about the image
    /// </summary>
    public class Image
    {
        [Key]
        public string? ImageId { get; set; }
        /// <summary>
        /// Title image
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Path where the pictures are stored
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// User ID which uploaded image
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// Navigation property
        /// </summary>
        [JsonIgnore]
        public User? User { get; set; }
    }
}

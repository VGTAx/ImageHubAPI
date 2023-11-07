namespace ImageHubAPI.DTOs
{
    /// <summary>
    /// DTO for adding friend
    /// </summary>
    public class AddFriendDto
    {
        /// <summary>
        /// User ID which send request
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// User ID to which the request is sent
        /// </summary>
        public string? FriendId { get; set; }
    }
}

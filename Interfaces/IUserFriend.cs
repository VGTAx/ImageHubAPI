namespace ImageHubAPI.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUserFriend<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddFriendAsync(T user);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> IsFriendAddAsync(string userId, string friendId);
    }
}

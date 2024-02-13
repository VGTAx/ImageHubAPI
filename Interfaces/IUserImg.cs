namespace ImageHubAPI.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserImg<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        Task<List<string>> GetImgByUserIdAsync(string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsImageAlreadyAddedAsync(string imgName, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SaveImageAsync(List<IFormFile> formFile, string path, T user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string? GetUploadPath(string userId);
    }
}

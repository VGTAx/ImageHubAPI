namespace ImageHubAPI.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Checks the availability of an email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Result checking the availability of an email.</returns>
        Task<bool> IsEmailAvailableAsync(string email);
    }
}

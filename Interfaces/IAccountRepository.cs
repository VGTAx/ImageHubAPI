namespace ImageHubAPI.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  public interface IAccountRepository
  {
    /// <summary>
    /// Checks the availability of an email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>Result checking the availability of an email.</returns>
    Task<bool> IsEmailAvailable(string email);    
  }
}

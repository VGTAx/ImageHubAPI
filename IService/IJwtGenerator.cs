using ImageHubAPI.Models;

namespace ImageHubAPI.IService
{
    /// <summary>
    /// Interface for generating JWT tokens.
    /// </summary>
    public interface IJwtGenerator
  {
    /// <summary>
    /// Generates a JWT token based on the provided credentials.
    /// </summary>
    /// <returns>Generated JWT tokenS</returns>
    string CreateToken(User user);
  }
}

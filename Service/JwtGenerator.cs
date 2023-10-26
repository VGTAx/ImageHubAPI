using ImageHubAPI.IService;
using ImageHubAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ImageHubAPI.Service
{
    /// <summary>
    /// Implementation of an <see cref="IJwtGenerator"/> for generating JWT tokens.
    /// </summary>
    public class JwtGenerator : IJwtGenerator
  {
    private readonly SymmetricSecurityKey _key;
    /// <summary>
    /// JwtGenerator constructor
    /// </summary>
    /// <param name="configuration"></param>
    public JwtGenerator(IConfiguration configuration)
    {
      _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]!));
    }
    /// <summary>
    /// Generates a JWT token based on the provided credentials.
    /// </summary>
    /// <param name="user">Data for generates a JWT token</param>
    /// <returns>Generated JWT token</returns>
    public string CreateToken(User user)
    {
      var claims = new List<Claim> { new Claim("UserID", user.Id!) };
      var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(7),
        SigningCredentials = credentials
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}

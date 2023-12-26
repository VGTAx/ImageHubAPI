using ImageHubAPI.Controllers;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace UnitTests
{
  public static class TestObjectFactory
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="returnClaimType"></param>
    /// <returns></returns>
    public static ControllerContext GetControllerContext(string? returnClaimType = null)
    {
      var fakeClaimsPrincipal = new Mock<ClaimsPrincipal>();

      if (returnClaimType != null)
      {
        fakeClaimsPrincipal
          .Setup(cp => cp.FindFirst("UserID"))
          .Returns(new Claim("UserID", "expectedValue"));
      }
      else
      {
        fakeClaimsPrincipal
        .Setup(cp => cp.FindFirst(""))
        .Returns((string _) => null);
      }

      // Создаем фейковый объект ControllerContext
      var fakeControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext
        {
          User = fakeClaimsPrincipal.Object,
        }
      };

      return fakeControllerContext;
    }

    /// <summary>
    /// Return <see cref="UserFriendController"/> object
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <param name="returnClaimType">Claim type for creating Controller Context</param>
    /// <returns></returns>
    public static UserFriendController GetUserFriendController(IUserFriendRepository<User> repository, string? returnClaimType = null)
    {
      return new UserFriendController(repository)
      {
        ControllerContext = TestObjectFactory.GetControllerContext(returnClaimType),
      };
    }

    public static UserImgController GetUserImageController(
      IUserImgRepository<User> imgRepository,
      IUserFriendRepository<User> friendRepository,
      IConfiguration configuration,
      IFriendshipRepository<Friendship> friendshipRepository,
      string? returnClaimType = null)
    {
      return new UserImgController(imgRepository, friendRepository, configuration, friendshipRepository)
      {
        ControllerContext = GetControllerContext(returnClaimType)
      };
    }

    public static Mock<UserManager<User>> GetUserManager()
    {
      return new Mock<UserManager<User>>(
        Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
    }

    public static Mock<SignInManager<User>> GetSignInManager()
    {
      return new Mock<SignInManager<User>>(GetUserManager().Object,
        new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object,
        new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);
    }
  }
}

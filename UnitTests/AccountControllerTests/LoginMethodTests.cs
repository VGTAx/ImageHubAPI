using ImageHubAPI.Controllers;
using ImageHubAPI.Interfaces;
using ImageHubAPI.IService;
using ImageHubAPI.Models;
using ImageHubAPI.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.AccountControllerTests
{
  [TestFixture]
  public class LoginMethodTests
  {
    [Test]
    public async Task Login_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var stubLogin = new Mock<Login>();
      var stubUserManager = TestObjectFactory.GetUserManager();
      var stubSignInManager = TestObjectFactory.GetSignInManager();
      var stubRepository = new Mock<IAccountRepository>();
      var stubJwtGenerator = new Mock<IJwtGenerator>();
      var stubUserStore = new Mock<IUserStore<User>>();
      var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object, stubJwtGenerator.Object, stubRepository.Object);

      controller.ModelState.AddModelError("ModelError", "Some model error");

      //Act
      var result = await controller.Login(stubLogin.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Login_IncorrectLoginOrPassword_ReturnUnauthorized()
    {
      //Arrange
      var stubLogin = new Mock<Login>();
      var stubUserManager = TestObjectFactory.GetUserManager();
      var stubSignInManager = TestObjectFactory.GetSignInManager();
      var stubRepository = new Mock<IAccountRepository>();
      var stubJwtGenerator = new Mock<IJwtGenerator>();
      var stubUserStore = new Mock<IUserStore<User>>();
      var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object, stubJwtGenerator.Object, stubRepository.Object);

      stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());

      stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
         .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

      //Act
      var result = await controller.Login(stubLogin.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    [Test]
    public async Task Login_UserNotFound_ReturnNotFound()
    {
      //Arrange
      var stubLogin = new Mock<Login>();
      var stubUserManager = TestObjectFactory.GetUserManager();
      var stubSignInManager = TestObjectFactory.GetSignInManager();
      var stubRepository = new Mock<IAccountRepository>();
      var stubJwtGenerator = new Mock<IJwtGenerator>();
      var stubUserStore = new Mock<IUserStore<User>>();
      var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object, stubJwtGenerator.Object, stubRepository.Object);

      stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(null as User);

      //Act
      var result = await controller.Login(stubLogin.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Login_JwtTokenNotCreated_ReturnBadRequest()
    {
      //Arrange
      var stubLogin = new Mock<Login>();
      var stubUserManager = TestObjectFactory.GetUserManager();
      var stubSignInManager = TestObjectFactory.GetSignInManager();
      var stubRepository = new Mock<IAccountRepository>();
      var stubJwtGenerator = new Mock<IJwtGenerator>();
      var stubUserStore = new Mock<IUserStore<User>>();
      var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object, stubJwtGenerator.Object, stubRepository.Object);

      stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());
      stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
        .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
      stubJwtGenerator.Setup(jg => jg.CreateToken(It.IsAny<User>()))
        .Returns(string.Empty);

      //Act
      var result = await controller.Login(stubLogin.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Login_UserLoginSuccessful_ReturnOk()
    {
      //Arrange
      var stubLogin = new Mock<Login>();
      var stubUserManager = TestObjectFactory.GetUserManager();
      var stubSignInManager = TestObjectFactory.GetSignInManager();
      var stubRepository = new Mock<IAccountRepository>();
      var stubJwtGenerator = new Mock<IJwtGenerator>();
      var stubUserStore = new Mock<IUserStore<User>>();
      var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object, stubJwtGenerator.Object, stubRepository.Object);
           
      stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());
      stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
        .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
      stubJwtGenerator.Setup(jg => jg.CreateToken(It.IsAny<User>()))
        .Returns("some_token");

      //Act
      var result = await controller.Login(stubLogin.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}
using ImageHubAPI.Controllers;
using ImageHubAPI.Data;
using ImageHubAPI.IService;
using ImageHubAPI.Models;
using ImageHubAPI.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;

namespace ImageHubAPI.UnitTests
{
  [TestFixture]
  public class AccountControllerTests
  {
    private AccountController _controller;
    private Mock<IImageHubContext> _contextMock;
    private Mock<SignInManager<User>> _signInManagerMock;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<IUserStore<User>> _userStoreMock;
    private Mock<IJwtGenerator> _jwtGeneratorMock;

    [SetUp]
    public async Task Setup()
    {
      var users = new List<User>
      {
        new User { Name = "User_1", Email = "username@example.com" },
        new User { Name = "User_2", Email = "username1@example.com" },
        new User { Name = "User_3", Email = "username2@example.com" }
      }.AsQueryable();

      _contextMock = new Mock<IImageHubContext>();
      _contextMock.Setup(x => x.Users)
        .ReturnsDbSet(new List<User>
          {
              new User { Email = "username@example.com" },
              new User { Email = "username1@example.com" },
              new User { Email = "username2@example.com" }
          }.AsQueryable()
        );

      _userManagerMock = new Mock<UserManager<User>>(
        Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

      _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
        new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object,
        new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);

      _userStoreMock = new Mock<IUserStore<User>>();
      _jwtGeneratorMock = new Mock<IJwtGenerator>();

      _controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object);
    }

    [Test]
    public async Task Registration_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var registrationMock = new Mock<Registration>();

      _controller.ModelState.AddModelError("ModelError", "Some model error");

      //Act
      var result = await _controller.Registration(registrationMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Registration_EmailNotAvailable_ReturnBadRequest()
    {
      //Arrange 
      var registration = new Registration
      {
        Email = "username@example.com",
        Name = "username",
        Password = "passworD1!",
        ConfirmPassword = "passworD2!"
      };  

      //Act
      var result = await _controller.Registration(registration);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Email should not available");
    }

    [Test]
    public async Task Registration_UserManagerCreateUserFalse_ReturnBadRequest()
    {
      //Arrange
      var registrationMock = new Mock<Registration>();

      _userManagerMock
        .Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Failed());

      _controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object);

      //Act
      var result = await _controller.Registration(registrationMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Registration_UserRegister_ReturnOk()
    {
      //Arrange
      var registrationMock = new Mock<Registration>();

      _userManagerMock
        .Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);

      _controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object);

      //Act
      var result = await _controller.Registration(registrationMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>(), "User was not registered", 123);
    }

    [Test]
    public async Task Login_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var loginMock = new Mock<Login>();

      _controller.ModelState.AddModelError("ModelError", "Some model error");

      //Act
      var result = await _controller.Login(loginMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Login_IncorrectLoginOrPassword_ReturnUnauthorized()
    {
      //Arrange
      var loginMock = new Mock<Login>();

      _signInManagerMock.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
         .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

      _controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object);
      //Act
      var result = await _controller.Login(loginMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    [Test]
    public async Task Login_UserLogin_ReturnOk()
    {
      //Arrange
      var loginMock = new Mock<Login>();

      _signInManagerMock.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
         .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

      _controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object);
      //Act
      var result = await _controller.Login(loginMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}
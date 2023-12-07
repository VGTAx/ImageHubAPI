using ImageHubAPI.Controllers;
using ImageHubAPI.Interfaces;
using ImageHubAPI.IService;
using ImageHubAPI.Models.Account;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.AccountControllerTests
{
  [TestFixture]
  public class RegistrationMethodTests
  {
    private AccountController _controller;
    private Mock<IAccountRepository> _repositoryMock;
    private Mock<SignInManager<User>> _signInManagerMock;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<IUserStore<User>> _userStoreMock;
    private Mock<IJwtGenerator> _jwtGeneratorMock;

    [SetUp]
    public void Setup()
    {

      _userManagerMock = new Mock<UserManager<User>>(
        Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);

      _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
        new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object,
        new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);

      _repositoryMock = new Mock<IAccountRepository>();
      _userStoreMock = new Mock<IUserStore<User>>();
      _jwtGeneratorMock = new Mock<IJwtGenerator>();

      _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object, _repositoryMock.Object);
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
      var regigstrationMock = new Mock<Registration>();

      _repositoryMock
        .Setup(x => x.IsEmailAvailable(It.IsAny<string>()))
        .ReturnsAsync(true);

      _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object, _repositoryMock.Object);

      //Act
      var result = await _controller.Registration(regigstrationMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Email should not available");
    }

    [Test]
    public async Task Registration_UserManagerCreateUserIsFalse_ReturnBadRequest()
    {
      //Arrange
      var registrationMock = new Mock<Registration>();

      _userManagerMock
        .Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Failed());

      _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object, _repositoryMock.Object);

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

      _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _userStoreMock.Object, _jwtGeneratorMock.Object, _repositoryMock.Object);

      //Act
      var result = await _controller.Registration(registrationMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>(), "User was not registered", 123);
    }
  }
}

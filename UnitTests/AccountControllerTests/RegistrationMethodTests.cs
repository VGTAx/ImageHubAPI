using ImageHubAPI.Controllers;
using ImageHubAPI.Interfaces;
using ImageHubAPI.IService;
using ImageHubAPI.Models;
using ImageHubAPI.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.AccountControllerTests;

[TestFixture]
public class RegistrationMethodTests
{
    [Test]
    public async Task Registration_ModelIsNotValid_ReturnBadRequest()
    {
        //Arrange 
        var stubRegistration = new Mock<Registration>();
        var stubUserManager = TestObjectFactory.GetUserManager();
        var stubSignInManager = TestObjectFactory.GetSignInManager();
        var stubRepository = new Mock<IAccount>();
        var stubJwtGenerator = new Mock<IJwtGenerator>();
        var stubUserStore = new Mock<IUserStore<User>>();
        var controller = new AccountController(stubUserManager.Object,
            stubSignInManager.Object,
            stubUserStore.Object,
            stubJwtGenerator.Object,
            stubRepository.Object);

        controller.ModelState.AddModelError("ModelError", "Some model error");

        //Act
        var result = await controller.Registration(stubRegistration.Object);

        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Registration_EmailNotAvailable_ReturnBadRequest()
    {
        //Arrange 
        var stubRegistration = new Mock<Registration>();
        var stubUserManager = TestObjectFactory.GetUserManager();
        var stubSignInManager = TestObjectFactory.GetSignInManager();
        var stubRepository = new Mock<IAccount>();
        var stubJwtGenerator = new Mock<IJwtGenerator>();
        var stubUserStore = new Mock<IUserStore<User>>();
        var controller = new AccountController(stubUserManager.Object, stubSignInManager.Object, stubUserStore.Object,
            stubJwtGenerator.Object, stubRepository.Object);

        stubRepository
            .Setup(x => x.IsEmailAvailableAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        //Act
        var result = await controller.Registration(stubRegistration.Object);

        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Registration_ShouldCallSetUserNameAsyncOnUserStore_WhenUserIsProvided()
    {
        //Arrange       
        var stubRegistration = new Mock<Registration>();
        var stubUserManager = TestObjectFactory.GetUserManager();
        var stubSignInManager = TestObjectFactory.GetSignInManager();
        var stubRepository = new Mock<IAccount>();
        var stubJwtGenerator = new Mock<IJwtGenerator>();
        var mockUserStore = new Mock<IUserStore<User>>();
        var controller = new AccountController(stubUserManager.Object,
            stubSignInManager.Object,
            mockUserStore.Object,
            stubJwtGenerator.Object,
            stubRepository.Object);

        stubUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        await controller.Registration(stubRegistration.Object);

        //Assert
        Mock.Get(mockUserStore.Object)
            .Verify(us => us.SetUserNameAsync(It.IsAny<User>(), It.IsAny<string>(), CancellationToken.None),
                Times.Once());
    }

    [Test]
    public async Task Registration_UserManagerCreateUserIsFalse_ReturnBadRequest()
    {
        //Arrange
        var stubRegistration = new Mock<Registration>();
        var stubUserManager = TestObjectFactory.GetUserManager();
        var stubSignInManager = TestObjectFactory.GetSignInManager();
        var stubRepository = new Mock<IAccount>();
        var stubJwtGenerator = new Mock<IJwtGenerator>();
        var stubUserStore = new Mock<IUserStore<User>>();
        var controller = new AccountController(stubUserManager.Object,
            stubSignInManager.Object,
            stubUserStore.Object,
            stubJwtGenerator.Object,
            stubRepository.Object);

        stubUserManager
            .Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        //Act
        var result = await controller.Registration(stubRegistration.Object);

        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Registration_UserRegister_ReturnOk()
    {
        //Arrange
        var stubRegistration = new Mock<Registration>();
        var stubUserManager = TestObjectFactory.GetUserManager();
        var stubSignInManager = TestObjectFactory.GetSignInManager();
        var stubRepository = new Mock<IAccount>();
        var stubJwtGenerator = new Mock<IJwtGenerator>();
        var stubUserStore = new Mock<IUserStore<User>>();
        var controller = new AccountController(stubUserManager.Object,
            stubSignInManager.Object,
            stubUserStore.Object,
            stubJwtGenerator.Object,
            stubRepository.Object);

        stubUserManager
            .Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        var result = await controller.Registration(stubRegistration.Object);

        //Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}
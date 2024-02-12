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
            var _stubLogin = new Mock<Login>();
            var _stubUserManager = TestObjectFactory.GetUserManager();
            var _stubSignInManager = TestObjectFactory.GetSignInManager();
            var _stubAccountService = new Mock<IAccount>();
            var _stubJwtGenerator = new Mock<IJwtGenerator>();
            var _stubUserStore = new Mock<IUserStore<User>>();
            var controller = new AccountController(_stubUserManager.Object, _stubSignInManager.Object, _stubUserStore.Object, _stubJwtGenerator.Object, _stubAccountService.Object);

            controller.ModelState.AddModelError("ModelError", "Some model error");

            //Act
            var result = await controller.Login(_stubLogin.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task Login_IncorrectLoginOrPassword_ReturnUnauthorized()
        {
            //Arrange
            var _stubLogin = new Mock<Login>();
            var _stubUserManager = TestObjectFactory.GetUserManager();
            var _stubSignInManager = TestObjectFactory.GetSignInManager();
            var _stubAccountService = new Mock<IAccount>();
            var _stubJwtGenerator = new Mock<IJwtGenerator>();
            var _stubUserStore = new Mock<IUserStore<User>>();
            var controller = new AccountController(_stubUserManager.Object, _stubSignInManager.Object, _stubUserStore.Object, _stubJwtGenerator.Object, _stubAccountService.Object);

            _stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
               .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            //Act
            var result = await controller.Login(_stubLogin.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task Login_UserNotFound_ReturnNotFound()
        {
            //Arrange
            var _stubLogin = new Mock<Login>();
            var _stubUserManager = TestObjectFactory.GetUserManager();
            var _stubSignInManager = TestObjectFactory.GetSignInManager();
            var _stubAccountService = new Mock<IAccount>();
            var _stubJwtGenerator = new Mock<IJwtGenerator>();
            var _stubUserStore = new Mock<IUserStore<User>>();
            var controller = new AccountController(_stubUserManager.Object, _stubSignInManager.Object, _stubUserStore.Object, _stubJwtGenerator.Object, _stubAccountService.Object);

            _stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as User);

            //Act
            var result = await controller.Login(_stubLogin.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Login_JwtTokenNotCreated_ReturnBadRequest()
        {
            //Arrange
            var _stubLogin = new Mock<Login>();
            var _stubUserManager = TestObjectFactory.GetUserManager();
            var _stubSignInManager = TestObjectFactory.GetSignInManager();
            var _stubAccountService = new Mock<IAccount>();
            var _stubJwtGenerator = new Mock<IJwtGenerator>();
            var _stubUserStore = new Mock<IUserStore<User>>();
            var controller = new AccountController(_stubUserManager.Object, _stubSignInManager.Object, _stubUserStore.Object, _stubJwtGenerator.Object, _stubAccountService.Object);

            _stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());
            _stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
              .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _stubJwtGenerator.Setup(jg => jg.CreateToken(It.IsAny<User>()))
              .Returns(string.Empty);

            //Act
            var result = await controller.Login(_stubLogin.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Login_UserLoginSuccessful_ReturnOk()
        {
            //Arrange
            var _stubLogin = new Mock<Login>();
            var _stubUserManager = TestObjectFactory.GetUserManager();
            var _stubSignInManager = TestObjectFactory.GetSignInManager();
            var _stubAccountService = new Mock<IAccount>();
            var _stubJwtGenerator = new Mock<IJwtGenerator>();
            var _stubUserStore = new Mock<IUserStore<User>>();
            var controller = new AccountController(_stubUserManager.Object, _stubSignInManager.Object, _stubUserStore.Object, _stubJwtGenerator.Object, _stubAccountService.Object);

            _stubUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());
            _stubSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
              .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _stubJwtGenerator.Setup(jg => jg.CreateToken(It.IsAny<User>()))
              .Returns("some_token");

            //Act
            var result = await controller.Login(_stubLogin.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
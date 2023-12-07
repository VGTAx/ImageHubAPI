using ImageHubAPI.Controllers;
using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.UserFriendControllerTests
{
    [TestFixture]
    public class GetUserByEmailMethodTests
    {
        private UserFriendController _userController;
        private Mock<IUserFriendRepository<User>> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IUserFriendRepository<User>>();

            _repositoryMock
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            _repositoryMock
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            _userController = new UserFriendController(_repositoryMock.Object)
            {
                ControllerContext = TestObjectFactory.GetControllerContext(),
            };
        }        

        [Test]
        public async Task GetUserByEmail_EmailIsNullOrEmpty_ReturnBadRequest()
        {
            //Arrange
            var emptyStr = string.Empty;

            //Act
            var result = await _userController.GetUserByEmail(emptyStr);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_UserNotFound_ReturnNotFound()
        {
            //Arrange
            var email = "email@mail.com";

            User? user = null;
            _repositoryMock
              .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(user!);

            _userController = new UserFriendController(_repositoryMock.Object);

            //Act
            var result = await _userController.GetUserByEmail(email);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_UserReceived_ReturnOk()
        {
            //Arrange
            var email = "email@mail.com";

            var user = new User();
            _repositoryMock
              .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(user);

            _userController = new UserFriendController(_repositoryMock.Object);

            //Act
            var result = await _userController.GetUserByEmail(email);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}

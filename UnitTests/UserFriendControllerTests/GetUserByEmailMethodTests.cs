using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.UserFriendControllerTests
{
    [TestFixture]
    public class GetUserByEmailMethodTests
    {
        [Test]
        public async Task GetUserByEmail_EmailIsNullOrEmpty_ReturnBadRequest()
        {
            //Arrange
            var email = string.Empty;
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            //Act
            var result = await controller.GetUserByEmail(email);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_UserNotFound_ReturnNotFound()
        {
            //Arrange
            const string email = "email@mail.com";
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var stubUserService = new Mock<IUser<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);
    
            stubUserService
              .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))!
              .ReturnsAsync(null as User);

            //Act
            var result = await controller.GetUserByEmail(email);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_UserReceived_ReturnOk()
        {
            //Arrange
            const string email = "email@mail.com";
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
              .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))!
              .ReturnsAsync(new User());

            //Act
            var result = await controller.GetUserByEmail(email);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}

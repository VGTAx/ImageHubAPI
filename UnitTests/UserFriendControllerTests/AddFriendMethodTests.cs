using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.UserFriendControllerTests
{
    [TestFixture]
    public class AddFriendMethodTests
    {
        [Test]
        public async Task AddFriend_ModelIsNotValid_ReturnBadRequest()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            controller.ModelState.AddModelError("someError", "someError");

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task AddFriend_UserIsNotExist_ReturnNotFound()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task AddFriend_UserIdIsNotValid_ReturnForbid()
        {
            //Assert
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = 
                TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, "UserID");

            stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Arrange
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task AddFriend_FriendHasAlreadyAdded_ReturnBadRequest()
        {
            //Assert
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Arrange
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        }

        [Test]
        public async Task AddFriend_UserRequesterNotFound_ReturnNotFound()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(null as User);

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());

        }

        [Test]
        public async Task AddFriend_ShouldCallAddFriendshipOnUser_WhenFriendIsProvided()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var mockUser = new Mock<User>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(mockUser.Object);
            stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            
            Mock.Get(mockUser.Object).Verify(u => u.AddFriendship(It.IsAny<Friendship>()));
        }

        [Test]
        public async Task AddFriend_ShouldCallAddFriendOnIUserFriendRepository_WhenFriendIsProvided()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var mockRepostitory = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, mockRepostitory.Object, null);

            stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(new User());
            mockRepostitory
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            Mock.Get(mockRepostitory.Object).Verify(u => u.AddFriendAsync(It.IsAny<User>()));
        }

        [Test]
        public async Task AddFriend_FriendWasAdded_ReturnOk()
        {
            //Arrange
            var stubAddFriendDto = new Mock<AddFriendDto>();
            var stubUserService = new Mock<IUser<User>>();
            var stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(stubUserService.Object, stubUserFriend.Object, null);

            stubUserService
               .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
               .ReturnsAsync(true);
            stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(new User());
            stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            var result = await controller.AddFriend(stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}

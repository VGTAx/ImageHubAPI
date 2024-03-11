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
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            controller.ModelState.AddModelError("someError", "someError");

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task AddFriend_UserIsNotExist_ReturnNotFound()
        {
            //Arrange
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            _stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task AddFriend_UserIdIsNotValid_ReturnForbid()
        {
            //Assert
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = 
                TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, "UserID");

            _stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Arrange
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task AddFriend_FriendHasAlreadyAdded_ReturnBadRequest()
        {
            //Assert
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            _stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Arrange
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        }

        [Test]
        public async Task AddFriend_UserRequesterNotFound_ReturnNotFound()
        {
            //Arrange
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            _stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(null as User);

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());

        }

        [Test]
        public async Task AddFriend_ShouldCallAddFriendshipOnUser_WhenFriendIsProvided()
        {
            //Arrange
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var _mockUser = new Mock<User>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            _stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(_mockUser.Object);
            _stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            
            Mock.Get(_mockUser.Object).Verify(u => u.AddFriendship(It.IsAny<Friendship>()));
        }

        [Test]
        public async Task AddFriend_ShouldCallAddFriendOnIUserFriendRepository_WhenFriendIsProvided()
        {
            //Arrange
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _mockRepostitory = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _mockRepostitory.Object, null);

            _stubUserService
              .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(new User());
            _mockRepostitory
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            Mock.Get(_mockRepostitory.Object).Verify(u => u.AddFriendAsync(It.IsAny<User>()));
        }

        [Test]
        public async Task AddFriend_FriendWasAdded_ReturnOk()
        {
            //Arrange
            var _stubAddFriendDto = new Mock<AddFriendDto>();
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserFriend = new Mock<IUserFriend<User>>();
            var controller = TestObjectFactory.GetUserFriendController(_stubUserService.Object, _stubUserFriend.Object, null);

            _stubUserService
               .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
               .ReturnsAsync(true);
            _stubUserService
              .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(new User());
            _stubUserFriend
              .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            //Act
            var result = await controller.AddFriend(_stubAddFriendDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}

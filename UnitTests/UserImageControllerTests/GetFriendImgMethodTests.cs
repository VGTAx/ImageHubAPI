using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.UserImageController
{
    [TestFixture]
    public class GetFriendImgMethodTests
    {
        [Test]
        public async Task GetFriendImg_FriedIdNullOrEmpty_ReturnBadRequest()
        {
            //Arrange
            var friendId = string.Empty;
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            var controller = TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetFriendImg_FriendIsNotExist_ReturnNotFound()
        {
            //Arrange
            var friendId = "id";
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectorty = new Mock<IDirectory>();

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller = TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectorty.Object);

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetFriendImg_UserIdIsNotValid_ReturnForbid()
        {
            //Arrange
            var friendId = "incorrectId";
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubFriendshipRepository
              .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
              .ReturnsAsync(null as Friendship);

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller = TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task GetFriendImg_FriendshipIsNull_ReturnForbid()
        {
            //Arrange
            var friendId = "id";
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubFriendshipRepository
              .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
              .ReturnsAsync(null as Friendship);

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller =
              TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task GetFriendImg_FriendHasNotImg_ReturnNotFound()
        {
            //Arrange
            var friendId = "id";
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubFriendshipRepository
              .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
              .ReturnsAsync(new Friendship());

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            stubImgRepository
              .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(new List<string>());

            var controller =
              TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetFriendImg_FriendImgIsGot_ReturnOk()
        {
            //Arrange
            var friendId = "id";
            var stubUserService = new Mock<IUser<User>>();
            var stubImgRepository = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            var images = new List<string>
            {
              "someAdressImg_1",
              "someAdressImg_2",
            };

            stubFriendshipRepository
              .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
              .ReturnsAsync(new Friendship());
            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubImgRepository
              .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))!
              .ReturnsAsync(images);

            var controller =
              TestObjectFactory.GetUserImageController(stubUserService.Object, stubImgRepository.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            var result = await controller.GetFriendImg(friendId);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}

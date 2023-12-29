using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.UserImageController
{
  public class GetUserImgMethodTests
  {
    [Test]
    public async Task GetUserImg_UserIdNullOrEmpty_ReturnBadRequest()
    {
      //Arrange
      var userId = string.Empty;
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();
      var _stubDirectory = new Mock<IDirectory>();

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

      //Act
      var result = await controller.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetUserImg_UserIsNotExist_ReturnNotFound()
    {
      //Arrange
      var userId = "id";
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();
      var _stubDirectory = new Mock<IDirectory>();

      _stubImgRepository
       .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
       .ReturnsAsync(false);

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

      //Act
      var result = await controller.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetUserImg_UserIdIsNotValid_ReturnForbid()
    {
      //Arrange
      var incorrectUserId = "incorrectId";

      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();
      var _stubDirectory = new Mock<IDirectory>();

      _stubImgRepository
       .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
       .ReturnsAsync(true);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

      //Act
      var result = await controller.GetUserImg(incorrectUserId);

      //Assert
      Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task GetUserImg_UserHasNotImages_ReturnNotFound()
    {
      //Arrange
      var expectedUserId = "expectedValue";
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();
      var _stubDirectory = new Mock<IDirectory>();

      _stubImgRepository
       .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
       .ReturnsAsync(true);

      _stubImgRepository
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new List<string>());

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

      //Act
      var result = await controller.GetUserImg(expectedUserId);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetUserImg_ImageIsGot_ReturnOk()
    {
      //Arrange
      var expectedUserId = "expectedValue";
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();
      var _stubDirectory = new Mock<IDirectory>();
      var images = new List<string>
      {
        "someAdressImg_1",
        "someAdressImg_2",
      };

      _stubImgRepository
       .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
       .ReturnsAsync(true);

      _stubImgRepository
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))
        .ReturnsAsync(images);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

      //Act
      var result = await controller.GetUserImg(expectedUserId);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}

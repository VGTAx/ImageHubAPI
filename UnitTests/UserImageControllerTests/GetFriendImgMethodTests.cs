using ImageHubAPI.Controllers;
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
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

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
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

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

      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubFriendshipRepository
        .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
        .ReturnsAsync(null as Friendship);

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

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
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubFriendshipRepository
        .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
        .ReturnsAsync(null as Friendship);

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      var controller =
        TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

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
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubFriendshipRepository
        .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
        .ReturnsAsync(new Friendship());

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      _stubImgRepository
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))!
        .ReturnsAsync(new List<string>());

      var controller =
        TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

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
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      var images = new List<string>
      {
        "someAdressImg_1",
        "someAdressImg_2",
      };

      _stubFriendshipRepository
        .Setup(fr => fr.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))!
        .ReturnsAsync(new Friendship());

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      _stubImgRepository
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))!
        .ReturnsAsync(images);

      var controller =
        TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

      //Act
      var result = await controller.GetFriendImg(friendId);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}

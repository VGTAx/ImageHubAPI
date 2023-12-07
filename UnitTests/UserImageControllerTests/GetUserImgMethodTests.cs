using ImageHubAPI.Controllers;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.UserImageController
{
  public class GetUserImgMethodTests
  {
    private Mock<IUserImgRepository<User>> _userImgRepositoryMock;
    private Mock<IUserFriendRepository<User>> _userFriendRepositoryMock;
    private Mock<IFriendshipRepository<Friendship>> _friendshipRepositoryMock;
    private Mock<IConfiguration> _configuration;
    private UserImgController _userImgController;

    [SetUp]
    public void Setup()
    {
      _userImgRepositoryMock = new Mock<IUserImgRepository<User>>();
      _userImgRepositoryMock
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);
      _userImgRepositoryMock
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(true);

      _userFriendRepositoryMock = new Mock<IUserFriendRepository<User>>();
      _userFriendRepositoryMock
        .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());

      _friendshipRepositoryMock = new Mock<IFriendshipRepository<Friendship>>();
      _friendshipRepositoryMock
        .Setup(f => f.GetFriendshipAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(new Friendship());

      var configSectionMock = new Mock<IConfigurationSection>();
      _configuration = new Mock<IConfiguration>();
      _configuration
        .Setup(x => x.GetSection(It.IsAny<string>()))
        .Returns(configSectionMock.Object);

      _userImgController = new UserImgController(_userImgRepositoryMock.Object, _userFriendRepositoryMock.Object, _configuration.Object, _friendshipRepositoryMock.Object)
      {
        ControllerContext = TestObjectFactory.GetControllerContext()
      };
    }

    [Test]
    public async Task GetUserImg_UserIdNullOrEmpty_ReturnBadRequest()
    {
      //Arrange
      var userId = string.Empty;

      //Act
      var result = await _userImgController.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetUserImg_UserIsNotExist_ReturnNotFound()
    {
      //Arrange
      var userId = "id";

      _userImgRepositoryMock
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(false);
      //Act
      var result = await _userImgController.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetUserImg_UserIdIsNotValid_ReturnForbid()
    {
      //Arrange
      var userId = "incorrectId";

      //Act
      var result = await _userImgController.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task GetUserImg_UserHasNotImages_ReturnNotFound()
    {
      //Arrange
      var userId = "expectedValue";
      _userImgRepositoryMock
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new List<string>());
      _userImgController.ControllerContext = TestObjectFactory.GetControllerContext("UserID");
      //Act
      var result = await _userImgController.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetUserImg_ImageIsGot_ReturnOk()
    {
      //Arrange
      var userId = "expectedValue";
      var images = new List<string>
      {
        "someAdressImg_1",
        "someAdressImg_2",
      };

      _userImgRepositoryMock
        .Setup(ui => ui.GetImgByUserIdAsync(It.IsAny<string>()))
        .ReturnsAsync(images);
      _userImgController.ControllerContext = TestObjectFactory.GetControllerContext("UserID");
      //Act
      var result = await _userImgController.GetUserImg(userId);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}

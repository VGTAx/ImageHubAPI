using ImageHubAPI.Controllers;
using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
  [TestFixture]
  public class UserFriendControllerTests
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
        ControllerContext = TestObjectFactory.CreateControllerContext(),
      };
    }

    [Test]
    public async Task AddFriend_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var addFriendDtoMock = new Mock<AddFriendDto>();

      _userController.ModelState.AddModelError("someError", "someError");
      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task AddFriend_UserIsNotExist_ReturnNotFound()
    {
      //Arrange
      var addFriendDtoMock = new Mock<AddFriendDto>();
      _repositoryMock
        .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task AddFriend_UserIdIsNotValid_ReturnForbid()
    {
      //Assert
      var addFriendDtoMock = new Mock<AddFriendDto>();
      _userController.ControllerContext = TestObjectFactory.CreateControllerContext("UserID");

      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Arrange
      Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task AddFriend_FriendHasAlreadyAdded_ReturnBadRequest()
    {
      //Assert
      var addFriendDtoMock = new Mock<AddFriendDto>();

      _repositoryMock
        .Setup(r => r.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      _repositoryMock
        .Setup(r => r.IsFriendAddAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(false);

      _userController = new UserFriendController(_repositoryMock.Object);

      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Arrange
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

    }

    [Test]
    public async Task AddFriend_GetUserById_ReturnNotFound()
    {
      //Arrange
      var addFriendDtoMock = new Mock<AddFriendDto>();

      User? user = null;

      _repositoryMock
        .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(user!);

      _userController = new UserFriendController(_repositoryMock.Object);

      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());

    }

    [Test]
    public async Task AddFriend_FriendWasAdded_ReturnOk()
    {
      //Arrange
      var addFriendDtoMock = new Mock<AddFriendDto>();
      var userMock = new Mock<User>();



      _repositoryMock
        .Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(userMock.Object);

      _userController = new UserFriendController(_repositoryMock.Object);

      //Act
      var result = await _userController.AddFriend(addFriendDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
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

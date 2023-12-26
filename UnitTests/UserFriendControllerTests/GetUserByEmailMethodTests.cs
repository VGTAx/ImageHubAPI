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
    [Test]
    public async Task GetUserByEmail_EmailIsNullOrEmpty_ReturnBadRequest()
    {
      //Arrange
      var emptyStr = string.Empty;
      var _stubRepository = new Mock<IUserFriendRepository<User>>();
      var controller = TestObjectFactory.GetUserFriendController(_stubRepository.Object, null);

      //Act
      var result = await controller.GetUserByEmail(emptyStr);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetUserByEmail_UserNotFound_ReturnNotFound()
    {
      //Arrange
      var email = "email@mail.com";
      var _stubRepository = new Mock<IUserFriendRepository<User>>();
      var controller = TestObjectFactory.GetUserFriendController(_stubRepository.Object, null);

      _stubRepository
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
      var email = "email@mail.com";
      var _stubRepository = new Mock<IUserFriendRepository<User>>();
      var controller = TestObjectFactory.GetUserFriendController(_stubRepository.Object, null);

      _stubRepository
        .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))!
        .ReturnsAsync(new User());

      //Act
      var result = await controller.GetUserByEmail(email);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
  }
}

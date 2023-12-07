using ImageHubAPI.Controllers;
using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.UserImageController
{
  [TestFixture]
  public class UploadImgMethodTests
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
    public async Task UploadImg_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();

      _userImgController.ModelState.AddModelError("someKey", "someMessage");

      //Act
      var result = await _userImgController.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task UploadImg_UserIsNotExist_ReturnNotFound()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();

      _userImgRepositoryMock
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

      //Act
      var result = await _userImgController.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UploadImg_UserIdIsNotValid_ReturnForbid()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      _userImgController.ControllerContext = TestObjectFactory.GetControllerContext("UserID");

      //Act
      var result = await _userImgController.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task UploadImg_ImageIsAlreadyAdded_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      _userImgRepositoryMock
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(false);

      //Act
      var result = await _userImgController.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UploadImg_NoImagesToUpload_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();

      //Act
      var result = await _userImgController.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    //[Test]
    //public async Task UploadImg_ImageIsUploaded_ReturnOk()
    //{
    //  //Arrange
    //  var uploadImgDtoMock = new UploadImgDto
    //  {
    //    Images = new List<IFormFile>()
    //    {
    //      new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
    //    },
    //    UserID = "expectedValue"
    //  };

    //  _userImgController.ControllerContext = TestObjectFactory.GetControllerContext("UserID");

    //  //Act
    //  var result = await _userImgController.UploadImg(uploadImgDtoMock);

    //  //Assert
    //  Assert.That(result, Is.InstanceOf<OkObjectResult>());
    //}
  }
}

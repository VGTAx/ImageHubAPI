using ImageHubAPI.DTOs;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.UserImageController
{
  [TestFixture]
  public class UploadImgMethodTests
  {
    [Test]
    public async Task UploadImg_ModelIsNotValid_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);
      controller.ModelState.AddModelError("someKey", "someMessage");

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task UploadImg_UserIsNotExist_ReturnNotFound()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

      var controller = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UploadImg_UserIdIsNotValid_ReturnForbid()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "NotValidId");

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task UploadImg_ImageIsAlreadyAdded_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);
      _stubImgRepository
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(false);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UploadImg_NoImagesToUpload_ReturnBadRequest()
    {
      //Arrange
      var uploadImgDtoMock = new Mock<UploadImgDto>();
      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object);

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock.Object);

      //Assert
      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UploadImg_ImageIsUploaded_ReturnOk()
    {
      //Arrange
      var uploadImgDtoMock = new UploadImgDto
      {
        Images = new List<IFormFile>()
        {
          new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
        },
        UserID = "expectedValue"
      };

      var _stubImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _stubImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);
      _stubFriendRepostitory
        .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());

      _stubImgRepository
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(true);

      var stubConfigSection = new Mock<IConfigurationSection>();
      stubConfigSection
        .Setup(cs => cs.Key)
        .Returns("someSection");
      stubConfigSection
        .Setup(cs => cs.Value)
        .Returns("someValueSection");
      _stubConfiguration
        .Setup(x => x.GetSection(It.IsAny<string>()))
        .Returns(stubConfigSection.Object);

      var controller
        = TestObjectFactory.GetUserImageController(_stubImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

      //Act
      var result = await controller.UploadImg(uploadImgDtoMock);

      //Assert
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }


    [Test]
    public async Task UploadImg_ShouldCallUpdateUserOnIUserImgRepository_WhenUserIsProvided()
    {
      //Arrange
      var uploadImgDtoMock = new UploadImgDto
      {
        Images = new List<IFormFile>()
        {
          new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
        },
        UserID = "expectedValue"
      };

      var _mockImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _mockImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);
      _stubFriendRepostitory
        .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());

      _mockImgRepository
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(true);

      var stubConfigSection = new Mock<IConfigurationSection>();
      stubConfigSection
        .Setup(cs => cs.Key)
        .Returns("someSection");
      stubConfigSection
        .Setup(cs => cs.Value)
        .Returns("someValueSection");
      _stubConfiguration
        .Setup(x => x.GetSection(It.IsAny<string>()))
        .Returns(stubConfigSection.Object);

      var controller
        = TestObjectFactory.GetUserImageController(_mockImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

      //Act
      await controller.UploadImg(uploadImgDtoMock);

      //Assert
      Mock.Get(_mockImgRepository.Object).Verify(x => x.UpdateUserWithImages(It.IsAny<User>()));
    }

    [Test]
    public async Task UploadImg_ShouldCallSaveChangeAsyncOnIUserImgRepository_WhenUserIsProvided()
    {
      //Arrange
      var uploadImgDtoMock = new UploadImgDto
      {
        Images = new List<IFormFile>()
        {
          new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
        },
        UserID = "expectedValue"
      };

      var _mockImgRepository = new Mock<IUserImgRepository<User>>();
      var _stubFriendRepostitory = new Mock<IUserFriendRepository<User>>();
      var _stubFriendshipRepository = new Mock<IFriendshipRepository<Friendship>>();
      var _stubConfiguration = new Mock<IConfiguration>();

      _mockImgRepository
        .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
        .ReturnsAsync(true);
      _stubFriendRepostitory
        .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(new User());

      _mockImgRepository
        .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(true);

      var stubConfigSection = new Mock<IConfigurationSection>();
      stubConfigSection
        .Setup(cs => cs.Key)
        .Returns("someSection");
      stubConfigSection
        .Setup(cs => cs.Value)
        .Returns("someValueSection");
      _stubConfiguration
        .Setup(x => x.GetSection(It.IsAny<string>()))
        .Returns(stubConfigSection.Object);

      var controller
        = TestObjectFactory.GetUserImageController(_mockImgRepository.Object, _stubFriendRepostitory.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, "UserID");

      //Act
      await controller.UploadImg(uploadImgDtoMock);

      //Assert
      Mock.Get(_mockImgRepository.Object).Verify(x => x.SaveChangesAsync());
    }
  }
}

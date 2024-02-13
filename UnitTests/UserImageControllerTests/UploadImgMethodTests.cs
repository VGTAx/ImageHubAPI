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
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUploadImgDto = new Mock<UploadImgDto>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();
            var controller = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

            controller.ModelState.AddModelError("someKey", "someMessage");

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UploadImg_UserIsNotExist_ReturnNotFound()
        {
            //Arrange
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUploadImgDto = new Mock<UploadImgDto>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UploadImg_UserIdIsNotValid_ReturnForbid()
        {
            //Arrange
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUploadImgDto = new Mock<UploadImgDto>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "NotValidId");

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task UploadImg_ImageIsAlreadyAdded_ReturnBadRequest()
        {
            //Arrange
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUploadImgDto = new Mock<UploadImgDto>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_NoImagesToUpload_ReturnBadRequest()
        {
            //Arrange
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUploadImgDto = new Mock<UploadImgDto>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_UploadPathIsNullOrEmpty_ReturnBadRequest()
        {
            var _stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(us => us.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            _stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("");

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_ImageIsUploaded_ReturnOk()
        {
            //Arrange
            var _stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            _stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UploadImg_ShouldCallCreateDirectoryOnDirectory_WhenDirectoryIsNotExist()
        {
            //Assert
            var _stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var _stubUserService = new Mock<IUser<User>>();
            var _stubUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _mockDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            _stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            _mockDirectory
              .Setup(d => d.Exists(It.IsAny<string>()))
              .Returns(false);
            _mockDirectory
              .Setup(d => d.CreateDirectory(It.IsAny<string>()))
              .Returns(new DirectoryInfo("some_path"));

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _mockDirectory.Object, "UserID");
            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockDirectory.Object).Verify(d => d.CreateDirectory(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallSaveImagesAsyncOnIUserImg_WhenImgsAndPathAreProvided()
        {
            //Arrange
            var _stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                    new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var _stubUserService = new Mock<IUser<User>>();
            var _stubDirectory = new Mock<IDirectory>();
            var _mockUserImages = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _mockUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            _mockUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            var controller
               = TestObjectFactory.GetUserImageController(_stubUserService.Object, _mockUserImages.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockUserImages.Object)
              .Verify(ui => ui.SaveImageAsync(It.IsAny<List<IFormFile>>(), It.IsAny<string>(), It.IsAny<User>()), Times.Once());
        }
    }
}

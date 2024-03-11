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
            var stubUserService = new Mock<IUser<User>>();
            var stubUploadImgDto = new Mock<UploadImgDto>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();
            var controller = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            controller.ModelState.AddModelError("someKey", "someMessage");

            //Act
            var result = await controller.UploadImg(stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UploadImg_UserIsNotExist_ReturnNotFound()
        {
            //Arrange
            var stubUserService = new Mock<IUser<User>>();
            var stubUploadImgDto = new Mock<UploadImgDto>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UploadImg_UserIdIsNotValid_ReturnForbid()
        {
            //Arrange
            var stubUserService = new Mock<IUser<User>>();
            var stubUploadImgDto = new Mock<UploadImgDto>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "NotValidId");

            //Act
            var result = await controller.UploadImg(stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task UploadImg_ImageIsAlreadyAdded_ReturnBadRequest()
        {
            //Arrange
            var stubUserService = new Mock<IUser<User>>();
            var stubUploadImgDto = new Mock<UploadImgDto>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_NoImagesToUpload_ReturnBadRequest()
        {
            //Arrange
            var stubUserService = new Mock<IUser<User>>();
            var stubUploadImgDto = new Mock<UploadImgDto>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(stubUploadImgDto.Object);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_UploadPathIsNullOrEmpty_ReturnBadRequest()
        {
            var stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var stubUserService = new Mock<IUser<User>>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(us => us.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(us => us.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("");

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            var result = await controller.UploadImg(stubUploadImgDto);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadImg_ImageIsUploaded_ReturnOk()
        {
            //Arrange
            var stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var stubUserService = new Mock<IUser<User>>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var stubDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            var result = await controller.UploadImg(stubUploadImgDto);

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UploadImg_ShouldCallCreateDirectoryOnDirectory_WhenDirectoryIsNotExist()
        {
            //Assert
            var stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                  new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var stubUserService = new Mock<IUser<User>>();
            var stubUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();
            var mockDirectory = new Mock<IDirectory>();

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            stubUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            stubUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            mockDirectory
              .Setup(d => d.Exists(It.IsAny<string>()))
              .Returns(false);
            mockDirectory
              .Setup(d => d.CreateDirectory(It.IsAny<string>()))
              .Returns(new DirectoryInfo("some_path"));

            var controller
              = TestObjectFactory.GetUserImageController(stubUserService.Object, stubUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, mockDirectory.Object, "UserID");
            //Act
            await controller.UploadImg(stubUploadImgDto);

            //Assert
            Mock.Get(mockDirectory.Object).Verify(d => d.CreateDirectory(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallSaveImagesAsyncOnIUserImg_WhenImgsAndPathAreProvided()
        {
            //Arrange
            var stubUploadImgDto = new UploadImgDto
            {
                Images = new List<IFormFile>()
                {
                    new FormFile(Stream.Null, 0, 0, "image1", "image1.jpg"),
                },
                UserID = "expectedValue"
            };
            var stubUserService = new Mock<IUser<User>>();
            var stubDirectory = new Mock<IDirectory>();
            var mockUserImages = new Mock<IUserImg<User>>();
            var stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var stubConfiguration = new Mock<IConfiguration>();

            stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            mockUserImages
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);
            mockUserImages
                .Setup(ui => ui.GetUploadPath(It.IsAny<string>()))
                .Returns("somePath");

            var controller
               = TestObjectFactory.GetUserImageController(stubUserService.Object, mockUserImages.Object, stubConfiguration.Object, stubFriendshipRepository.Object, stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(stubUploadImgDto);

            //Assert
            Mock.Get(mockUserImages.Object)
              .Verify(ui => ui.SaveImageAsync(It.IsAny<List<IFormFile>>(), It.IsAny<string>(), It.IsAny<User>()), Times.Once());
        }
    }
}

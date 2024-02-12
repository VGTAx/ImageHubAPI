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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();
            var controller = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "NotValidId");

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object);

            //Act
            var result = await controller.UploadImg(_stubUploadImgDto.Object);

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

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
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubConfigSection = TestObjectFactory.GetConfigurationSection();
            var _mockDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _stubImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            _stubConfiguration
              .Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(_stubConfigSection.Object);

            _mockDirectory
              .Setup(d => d.Exists(It.IsAny<string>()))
              .Returns(false);
            _mockDirectory
              .Setup(d => d.CreateDirectory(It.IsAny<string>()))
              .Returns(new DirectoryInfo("some_path"));

            var controller
              = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _mockDirectory.Object, "UserID");
            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockDirectory.Object).Verify(d => d.CreateDirectory(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallUpdateUserOnIUserImgRepository_WhenUserIsProvided()
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
            var _mockImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubConfigSection = TestObjectFactory.GetConfigurationSection();
            var _stubDirectory = new Mock<IDirectory>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _mockImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            _stubConfiguration
              .Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(_stubConfigSection.Object);

            _stubDirectory
              .Setup(x => x.Exists(It.IsAny<string>()))
              .Returns(true);
            _stubDirectory
              .Setup(d => d.CreateDirectory(It.IsAny<string>()))
              .Returns(() => new DirectoryInfo(It.IsAny<string>()));
            var controller
               = TestObjectFactory.GetUserImageController(_stubUserService.Object, _mockImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockImgRepository.Object).Verify(x => x.UpdateUserWithImages(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallSaveChangeAsyncOnIUserImgRepository_WhenUserIsProvided()
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
            var _mockImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubConfigSection = TestObjectFactory.GetConfigurationSection();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _mockImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            _stubConfiguration
              .Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(_stubConfigSection.Object);

            var controller
               = TestObjectFactory.GetUserImageController(_stubUserService.Object, _mockImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockImgRepository.Object).Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallAddImagesOnUser_WhenImageIsProvided()
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
            var _stubImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubConfigSection = TestObjectFactory.GetConfigurationSection();
            var _mockUser = new Mock<User>();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(_mockUser.Object);

            _stubImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            _stubConfiguration
              .Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(_stubConfigSection.Object);

            var controller
               = TestObjectFactory.GetUserImageController(_stubUserService.Object, _stubImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockUser.Object).Verify(x => x.AddImages(It.IsAny<Image>()), Times.Once());
        }

        [Test]
        public async Task UploadImg_ShouldCallSaveImageAsyncOnIUserImgRepositoru_WhenImgAndPathAreProvided()
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
            var _mockImgRepository = new Mock<IUserImg<User>>();
            var _stubFriendshipRepository = new Mock<IFriendship<Friendship>>();
            var _stubConfiguration = new Mock<IConfiguration>();
            var _stubConfigSection = TestObjectFactory.GetConfigurationSection();

            _stubUserService
              .Setup(ui => ui.IsUserExistAsync(It.IsAny<string>()))
              .ReturnsAsync(true);
            _stubUserService
              .Setup(ur => ur.GetUserByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

            _mockImgRepository
              .Setup(ui => ui.IsImageAlreadyAddedAsync(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            _mockImgRepository
              .Setup(ui => ui.SaveImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
              .Returns(Task.CompletedTask);

            _stubConfiguration
              .Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(_stubConfigSection.Object);

            var controller
               = TestObjectFactory.GetUserImageController(_stubUserService.Object, _mockImgRepository.Object, _stubConfiguration.Object, _stubFriendshipRepository.Object, _stubDirectory.Object, "UserID");

            //Act
            await controller.UploadImg(_stubUploadImgDto);

            //Assert
            Mock.Get(_mockImgRepository.Object)
              .Verify(ui => ui.SaveImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once());
        }
    }
}

using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TourManagement.Web.Pages.Users;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Web.ViewModels;
using FluentAssertions;

namespace TourManagement.Web.Tests.Pages.Users
{
    public class RegisterModelTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<RegisterModel>> _mockLogger;
        private readonly RegisterModel _registerModel;

        public RegisterModelTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<RegisterModel>>();
            _registerModel = new RegisterModel(_mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            var logger = new Mock<ILogger<RegisterModel>>();

            // Act
            var model = new RegisterModel(userService.Object, logger.Object);

            // Assert
            model.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullUserService_ShouldThrowArgumentNullException()
        {
            // Arrange
            IUserService userService = null!;
            var logger = new Mock<ILogger<RegisterModel>>();

            // Act
            Action act = () => new RegisterModel(userService, logger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            ILogger<RegisterModel> logger = null!;

            // Act
            Action act = () => new RegisterModel(userService.Object, logger);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RegisterViewModel_Property_ShouldInitializeAsNewUserRegisterViewModel()
        {
            // Arrange & Act
            var model = new RegisterModel(_mockUserService.Object, _mockLogger.Object);

            // Assert
            model.RegisterViewModel.Should().NotBeNull();
            model.RegisterViewModel.Should().BeOfType<UserRegisterViewModel>();
        }

        [Fact]
        public void OnGet_ShouldExecute_WithoutException()
        {
            // Arrange
            var model = new RegisterModel(_mockUserService.Object, _mockLogger.Object);

            // Act
            Action act = () => model.OnGet();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidModelState_ShouldReturnPageResult()
        {
            // Arrange
            _registerModel.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_ShouldCallCreateAsync()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890"
            };

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserDto { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" });

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            _mockUserService.Verify(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_ShouldRedirectToLogin()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserDto { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" });

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<RedirectToPageResult>();
            var redirectResult = result as RedirectToPageResult;
            redirectResult!.PageName.Should().Be("./Login");
        }

        [Fact]
        public async Task OnPostAsync_WithValidData_ShouldSetSuccessMessage()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserDto { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" });

            // Mock TempData
            _registerModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            // Act
            await _registerModel.OnPostAsync();

            // Assert
            _registerModel.TempData["SuccessMessage"].Should().Be("Registration successful! Please login.");
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidOperationException_ShouldLogWarning()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var exception = new InvalidOperationException("Email already exists");
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _registerModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Registration failed - email already exists")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidOperationException_ShouldReturnPageResult()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var exception = new InvalidOperationException("Email already exists");
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidOperationException_ShouldAddModelError()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var exception = new InvalidOperationException("Email already exists");
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _registerModel.OnPostAsync();

            // Assert
            _registerModel.ModelState.Should().ContainKey(string.Empty);
            _registerModel.ModelState[string.Empty]!.Errors.Should().HaveCount(1);
            _registerModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("Email already exists");
        }

        [Fact]
        public async Task OnPostAsync_WithGeneralException_ShouldLogError()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var exception = new Exception("Database error");
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _registerModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error during registration")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithGeneralException_ShouldReturnPageResult()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var exception = new Exception("Database error");
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_ShouldMapViewModelToDto()
        {
            // Arrange
            _registerModel.RegisterViewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890"
            };

            UserCreateDto? capturedDto = null;
            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                .Callback<UserCreateDto>(dto => capturedDto = dto)
                .ReturnsAsync(new UserDto { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890" });

            // Act
            await _registerModel.OnPostAsync();

            // Assert
            capturedDto.Should().NotBeNull();
            capturedDto!.Email.Should().Be("test@example.com");
            capturedDto.Password.Should().Be("Password123!");
            capturedDto.FirstName.Should().Be("John");
            capturedDto.LastName.Should().Be("Doe");
            capturedDto.PhoneNumber.Should().Be("1234567890");
        }

        [Fact]
        public void RegisterModel_ShouldInheritFromPageModel()
        {
            // Arrange & Act
            var model = new RegisterModel(_mockUserService.Object, _mockLogger.Object);

            // Assert
            model.Should().BeAssignableTo<PageModel>();
        }
    }
}

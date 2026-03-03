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
    public class LoginModelTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<LoginModel>> _mockLogger;
        private readonly LoginModel _loginModel;
        private readonly Mock<ISession> _mockSession;

        public LoginModelTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<LoginModel>>();
            _mockSession = new Mock<ISession>();
            
            _loginModel = new LoginModel(_mockUserService.Object, _mockLogger.Object);
            
            // Setup HttpContext and Session
            var httpContext = new DefaultHttpContext();
            httpContext.Session = _mockSession.Object;
            _loginModel.PageContext.HttpContext = httpContext;
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            var logger = new Mock<ILogger<LoginModel>>();

            // Act
            var model = new LoginModel(userService.Object, logger.Object);

            // Assert
            model.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullUserService_ShouldThrowArgumentNullException()
        {
            // Arrange
            IUserService userService = null!;
            var logger = new Mock<ILogger<LoginModel>>();

            // Act
            Action act = () => new LoginModel(userService, logger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            ILogger<LoginModel> logger = null!;

            // Act
            Action act = () => new LoginModel(userService.Object, logger);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LoginViewModel_Property_ShouldInitializeAsNewUserLoginViewModel()
        {
            // Arrange & Act
            var model = new LoginModel(_mockUserService.Object, _mockLogger.Object);

            // Assert
            model.LoginViewModel.Should().NotBeNull();
            model.LoginViewModel.Should().BeOfType<UserLoginViewModel>();
        }

        [Fact]
        public void OnGet_ShouldExecute_WithoutException()
        {
            // Arrange
            var model = new LoginModel(_mockUserService.Object, _mockLogger.Object);

            // Act
            Action act = () => model.OnGet();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidModelState_ShouldReturnPageResult()
        {
            // Arrange
            _loginModel.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithValidCredentials_ShouldCallValidateLoginAsync()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userDto);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            _mockUserService.Verify(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithValidCredentials_ShouldRedirectToIndex()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userDto);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<RedirectToPageResult>();
            var redirectResult = result as RedirectToPageResult;
            redirectResult!.PageName.Should().Be("/Index");
        }

        [Fact]
        public async Task OnPostAsync_WithValidCredentials_ShouldSetSessionUserId()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userDto);

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _mockSession.Verify(s => s.Set("UserId", It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithValidCredentials_ShouldSetSessionUserEmail()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userDto);

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _mockSession.Verify(s => s.Set("UserEmail", It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidCredentials_ShouldReturnPageResult()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto?)null);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidCredentials_ShouldAddModelError()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto?)null);

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _loginModel.ModelState.Should().ContainKey(string.Empty);
            _loginModel.ModelState[string.Empty]!.Errors.Should().HaveCount(1);
            _loginModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("Invalid email or password.");
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldLogError()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var exception = new Exception("Database error");
            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error during login")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldReturnPageResult()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var exception = new Exception("Database error");
            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldAddModelError()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var exception = new Exception("Database error");
            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _loginModel.ModelState.Should().ContainKey(string.Empty);
            _loginModel.ModelState[string.Empty]!.Errors.Should().HaveCount(1);
            _loginModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("An error occurred during login.");
        }

        [Fact]
        public async Task OnPostAsync_ShouldMapViewModelToDto()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            UserLoginDto? capturedDto = null;
            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .Callback<UserLoginDto>(dto => capturedDto = dto)
                .ReturnsAsync(new UserDto { Id = 1, Email = "test@example.com" });

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            capturedDto.Should().NotBeNull();
            capturedDto!.Email.Should().Be("test@example.com");
            capturedDto.Password.Should().Be("Password123!");
        }

        [Fact]
        public void LoginModel_ShouldInheritFromPageModel()
        {
            // Arrange & Act
            var model = new LoginModel(_mockUserService.Object, _mockLogger.Object);

            // Assert
            model.Should().BeAssignableTo<PageModel>();
        }

        [Fact]
        public async Task OnPostAsync_WithValidCredentials_ShouldSetSuccessMessage()
        {
            // Arrange
            _loginModel.LoginViewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserService.Setup(s => s.ValidateLoginAsync(It.IsAny<UserLoginDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userDto);

            // Mock TempData
            _loginModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _loginModel.TempData["SuccessMessage"].Should().Be("Login successful!");
        }
    }
}

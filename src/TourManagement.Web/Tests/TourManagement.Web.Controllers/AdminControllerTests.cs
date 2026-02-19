using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Controllers;
using TourManagement.Application.Interfaces;
using TourManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.Web.Tests.Controllers;

public class AdminControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<IBookingService> _mockBookingService;
    private readonly Mock<ILogger<AdminController>> _mockLogger;
    private readonly AdminController _controller;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;

    public AdminControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockTourService = new Mock<ITourService>();
        _mockBookingService = new Mock<IBookingService>();
        _mockLogger = new Mock<ILogger<AdminController>>();
        
        _controller = new AdminController(
            _mockUserService.Object,
            _mockTourService.Object,
            _mockBookingService.Object,
            _mockLogger.Object);

        _mockHttpContext = new Mock<HttpContext>();
        _mockSession = new Mock<ISession>();
        _mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _mockHttpContext.Object
        };
    }

    [Fact]
    public void Constructor_WithNullUserService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AdminController(
            null!,
            _mockTourService.Object,
            _mockBookingService.Object,
            _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullTourService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AdminController(
            _mockUserService.Object,
            null!,
            _mockBookingService.Object,
            _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullBookingService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AdminController(
            _mockUserService.Object,
            _mockTourService.Object,
            null!,
            _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AdminController(
            _mockUserService.Object,
            _mockTourService.Object,
            _mockBookingService.Object,
            null!));
    }

    [Fact]
    public void Login_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Login();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Login_Post_WithValidAdminCredentials_RedirectsToProfile()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "admin@test.com", Password = "password" };
        var loginResult = new LoginResponseDto
        {
            Success = true,
            User = new UserDto { Id = 1, Email = "admin@test.com", Role = "Admin" }
        };
        _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
            .ReturnsAsync(loginResult);

        var sessionData = new Dictionary<string, byte[]>();
        _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => sessionData[key] = value);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Profile", redirectResult.ActionName);
    }

    [Fact]
    public async Task Login_Post_WithInvalidCredentials_ReturnsViewWithError()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "admin@test.com", Password = "wrong" };
        var loginResult = new LoginResponseDto { Success = false };
        _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
            .ReturnsAsync(loginResult);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(loginDto, viewResult.Model);
    }

    [Fact]
    public async Task Login_Post_WithNonAdminRole_ReturnsViewWithError()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@test.com", Password = "password" };
        var loginResult = new LoginResponseDto
        {
            Success = true,
            User = new UserDto { Id = 1, Email = "user@test.com", Role = "User" }
        };
        _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
            .ReturnsAsync(loginResult);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(loginDto, viewResult.Model);
    }

    [Fact]
    public async Task Login_Post_WithInvalidModelState_ReturnsView()
    {
        // Arrange
        var loginDto = new LoginDto();
        _controller.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(loginDto, viewResult.Model);
    }

    [Fact]
    public async Task Profile_WithValidAdminSession_ReturnsViewWithUser()
    {
        // Arrange
        var userId = 1;
        var userDto = new UserDto { Id = userId, Email = "admin@test.com", Role = "Admin" };
        
        _mockSession.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(userId);
                return true;
            });
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("Admin");
                return true;
            });

        _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(userDto);
        _mockTourService.Setup(x => x.GetAllToursAsync())
            .ReturnsAsync(new List<TourDto>());
        _mockBookingService.Setup(x => x.GetAllBookingsAsync())
            .ReturnsAsync(new List<BookingDto>());
        _mockUserService.Setup(x => x.GetAllUsersAsync())
            .ReturnsAsync(new List<UserDto>());

        // Act
        var result = await _controller.Profile();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(userDto, viewResult.Model);
    }

    [Fact]
    public async Task Profile_WithoutSession_RedirectsToLogin()
    {
        // Arrange
        _mockSession.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns(false);

        // Act
        var result = await _controller.Profile();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    [Fact]
    public async Task Profile_WithNonAdminRole_RedirectsToLogin()
    {
        // Arrange
        var userId = 1;
        _mockSession.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(userId);
                return true;
            });
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("User");
                return true;
            });

        // Act
        var result = await _controller.Profile();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    [Fact]
    public async Task Dashboard_WithAdminRole_ReturnsViewWithStatistics()
    {
        // Arrange
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("Admin");
                return true;
            });

        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, IsActive = true },
            new TourDto { Id = 2, IsActive = false }
        };
        var bookings = new List<BookingDto>
        {
            new BookingDto { Id = 1, Status = "Pending" },
            new BookingDto { Id = 2, Status = "Confirmed" },
            new BookingDto { Id = 3, Status = "Cancelled" }
        };
        var users = new List<UserDto>
        {
            new UserDto { Id = 1, IsActive = true },
            new UserDto { Id = 2, IsActive = true }
        };

        _mockTourService.Setup(x => x.GetAllToursAsync()).ReturnsAsync(tours);
        _mockBookingService.Setup(x => x.GetAllBookingsAsync()).ReturnsAsync(bookings);
        _mockUserService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.Dashboard();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(2, _controller.ViewBag.TotalTours);
        Assert.Equal(1, _controller.ViewBag.ActiveTours);
        Assert.Equal(3, _controller.ViewBag.TotalBookings);
        Assert.Equal(1, _controller.ViewBag.PendingBookings);
        Assert.Equal(1, _controller.ViewBag.ConfirmedBookings);
        Assert.Equal(1, _controller.ViewBag.CancelledBookings);
        Assert.Equal(2, _controller.ViewBag.TotalUsers);
        Assert.Equal(2, _controller.ViewBag.ActiveUsers);
    }

    [Fact]
    public async Task Dashboard_WithNonAdminRole_RedirectsToLogin()
    {
        // Arrange
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("User");
                return true;
            });

        // Act
        var result = await _controller.Dashboard();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    [Fact]
    public async Task Dashboard_WithException_ReturnsViewWithError()
    {
        // Arrange
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("Admin");
                return true;
            });
        _mockTourService.Setup(x => x.GetAllToursAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Dashboard();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}

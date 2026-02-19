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
using System.Threading.Tasks;

namespace TourManagement.Web.Tests.Controllers;

public class BookingControllerTests
{
    private readonly Mock<IBookingService> _mockBookingService;
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<ILogger<BookingController>> _mockLogger;
    private readonly BookingController _controller;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;

    public BookingControllerTests()
    {
        _mockBookingService = new Mock<IBookingService>();
        _mockTourService = new Mock<ITourService>();
        _mockLogger = new Mock<ILogger<BookingController>>();
        
        _controller = new BookingController(
            _mockBookingService.Object,
            _mockTourService.Object,
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
    public void Constructor_WithNullBookingService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BookingController(
            null!,
            _mockTourService.Object,
            _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullTourService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BookingController(
            _mockBookingService.Object,
            null!,
            _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BookingController(
            _mockBookingService.Object,
            _mockTourService.Object,
            null!));
    }

    [Fact]
    public async Task Index_WithAdminRole_ReturnsAllBookings()
    {
        // Arrange
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("Admin");
                return true;
            });

        var bookings = new List<BookingDto>
        {
            new BookingDto { Id = 1, UserId = 1 },
            new BookingDto { Id = 2, UserId = 2 }
        };
        _mockBookingService.Setup(x => x.GetAllBookingsAsync()).ReturnsAsync(bookings);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(viewResult.Model);
        Assert.Equal(2, ((List<BookingDto>)model).Count);
    }

    [Fact]
    public async Task Index_WithUserRole_ReturnsUserBookings()
    {
        // Arrange
        var userId = 1;
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("User");
                return true;
            });
        _mockSession.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(userId);
                return true;
            });

        var bookings = new List<BookingDto>
        {
            new BookingDto { Id = 1, UserId = userId }
        };
        _mockBookingService.Setup(x => x.GetBookingsByUserIdAsync(userId)).ReturnsAsync(bookings);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task MyBookings_WithValidSession_ReturnsUserBookings()
    {
        // Arrange
        var userId = 1;
        _mockSession.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(userId);
                return true;
            });

        var bookings = new List<BookingDto>
        {
            new BookingDto { Id = 1, UserId = userId }
        };
        _mockBookingService.Setup(x => x.GetBookingsByUserIdAsync(userId)).ReturnsAsync(bookings);

        // Act
        var result = await _controller.MyBookings();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_WithValidId_ReturnsViewWithBooking()
    {
        // Arrange
        var bookingId = 1;
        var userId = 1;
        var booking = new BookingDto { Id = bookingId, UserId = userId };
        
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

        _mockBookingService.Setup(x => x.GetBookingByIdAsync(bookingId)).ReturnsAsync(booking);

        // Act
        var result = await _controller.Details(bookingId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<BookingDto>(viewResult.Model);
        Assert.Equal(bookingId, model.Id);
    }

    [Fact]
    public async Task Cancel_WithValidBooking_RedirectsToMyBookings()
    {
        // Arrange
        var bookingId = 1;
        var userId = 1;
        var booking = new BookingDto { Id = bookingId, UserId = userId };
        
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

        _mockBookingService.Setup(x => x.GetBookingByIdAsync(bookingId)).ReturnsAsync(booking);
        _mockBookingService.Setup(x => x.CancelBookingAsync(bookingId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Cancel(bookingId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("MyBookings", redirectResult.ActionName);
    }

    [Fact]
    public async Task UpdateStatus_WithAdminRole_UpdatesStatusAndRedirectsToIndex()
    {
        // Arrange
        var bookingId = 1;
        var status = "Confirmed";
        
        _mockSession.Setup(x => x.TryGetValue("UserRole", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = System.Text.Encoding.UTF8.GetBytes("Admin");
                return true;
            });

        _mockBookingService.Setup(x => x.UpdateBookingStatusAsync(It.IsAny<UpdateBookingStatusDto>()))
            .ReturnsAsync(new BookingDto { Id = bookingId, Status = status });

        // Act
        var result = await _controller.UpdateStatus(bookingId, status);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}

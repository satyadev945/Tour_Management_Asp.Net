using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Controllers;
using TourManagement.Application.Interfaces;
using TourManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TourManagement.Web.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockLogger = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_mockTourService.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullTourService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HomeController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HomeController(_mockTourService.Object, null!));
    }

    [Fact]
    public async Task Index_ReturnsViewWithActiveTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour 1", IsActive = true },
            new TourDto { Id = 2, TourName = "Tour 2", IsActive = true }
        };
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync(tours);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Equal(2, ((List<TourDto>)model).Count);
    }

    [Fact]
    public async Task Index_WithEmptyTours_ReturnsViewWithEmptyList()
    {
        // Arrange
        var tours = new List<TourDto>();
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync(tours);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Index_WithException_ReturnsErrorView()
    {
        // Arrange
        _mockTourService.Setup(x => x.GetActiveToursAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewResult()
    {
        // Act
        var result = _controller.Error();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_CallsTourServiceGetActiveTours()
    {
        // Arrange
        var tours = new List<TourDto>();
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync(tours);

        // Act
        await _controller.Index();

        // Assert
        _mockTourService.Verify(x => x.GetActiveToursAsync(), Times.Once);
    }

    [Fact]
    public async Task Index_WithNullTours_ReturnsView()
    {
        // Arrange
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync((IEnumerable<TourDto>)null!);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_WithMultipleTours_ReturnsAllTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour 1", IsActive = true },
            new TourDto { Id = 2, TourName = "Tour 2", IsActive = true },
            new TourDto { Id = 3, TourName = "Tour 3", IsActive = true }
        };
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync(tours);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Equal(3, ((List<TourDto>)model).Count);
    }

    [Fact]
    public void Privacy_DoesNotThrowException()
    {
        // Act
        var exception = Record.Exception(() => _controller.Privacy());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Error_DoesNotThrowException()
    {
        // Act
        var exception = Record.Exception(() => _controller.Error());

        // Assert
        Assert.Null(exception);
    }
}

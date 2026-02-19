using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Controllers;
using TourManagement.Application.Interfaces;
using TourManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TourManagement.Web.Tests.Controllers;

public class TourControllerTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<ILogger<TourController>> _mockLogger;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly TourController _controller;

    public TourControllerTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockLogger = new Mock<ILogger<TourController>>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockEnvironment.Setup(x => x.WebRootPath).Returns("/test/wwwroot");

        _controller = new TourController(
            _mockTourService.Object,
            _mockLogger.Object,
            _mockEnvironment.Object);
    }

    [Fact]
    public void Constructor_WithNullTourService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TourController(
            null!,
            _mockLogger.Object,
            _mockEnvironment.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TourController(
            _mockTourService.Object,
            null!,
            _mockEnvironment.Object));
    }

    [Fact]
    public void Constructor_WithNullEnvironment_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TourController(
            _mockTourService.Object,
            _mockLogger.Object,
            null!));
    }

    [Fact]
    public async Task Index_ReturnsViewWithTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour 1" },
            new TourDto { Id = 2, TourName = "Tour 2" }
        };
        _mockTourService.Setup(x => x.GetAllToursAsync()).ReturnsAsync(tours);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Equal(2, ((List<TourDto>)model).Count);
    }

    [Fact]
    public async Task Index_WithException_ReturnsViewWithEmptyList()
    {
        // Arrange
        _mockTourService.Setup(x => x.GetAllToursAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Display_ReturnsViewWithActiveTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Active Tour", IsActive = true }
        };
        _mockTourService.Setup(x => x.GetActiveToursAsync()).ReturnsAsync(tours);

        // Act
        var result = await _controller.Display();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_WithValidId_ReturnsViewWithTour()
    {
        // Arrange
        var tourId = 1;
        var tour = new TourDto { Id = tourId, TourName = "Test Tour" };
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync(tour);

        // Act
        var result = await _controller.Details(tourId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TourDto>(viewResult.Model);
        Assert.Equal(tourId, model.Id);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync((TourDto?)null);

        // Act
        var result = await _controller.Details(tourId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Get_ReturnsView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_WithValidData_RedirectsToIndex()
    {
        // Arrange
        var createDto = new CreateTourDto
        {
            TourName = "New Tour",
            Place = "Test Place",
            Days = 5,
            Price = 1000
        };
        _mockTourService.Setup(x => x.CreateTourAsync(It.IsAny<CreateTourDto>()))
            .ReturnsAsync(new TourDto { Id = 1 });

        // Act
        var result = await _controller.Create(createDto, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_Post_WithInvalidModelState_ReturnsView()
    {
        // Arrange
        var createDto = new CreateTourDto();
        _controller.ModelState.AddModelError("TourName", "Required");

        // Act
        var result = await _controller.Create(createDto, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Get_WithValidId_ReturnsViewWithTour()
    {
        // Arrange
        var tourId = 1;
        var tour = new TourDto
        {
            Id = tourId,
            TourName = "Test Tour",
            Place = "Test Place",
            Days = 5,
            Price = 1000
        };
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync(tour);

        // Act
        var result = await _controller.Edit(tourId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateTourDto>(viewResult.Model);
        Assert.Equal(tourId, model.Id);
        Assert.Equal(tour.TourName, model.TourName);
    }

    [Fact]
    public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync((TourDto?)null);

        // Act
        var result = await _controller.Edit(tourId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithValidData_RedirectsToIndex()
    {
        // Arrange
        var updateDto = new UpdateTourDto
        {
            Id = 1,
            TourName = "Updated Tour",
            Place = "Test Place",
            Days = 5,
            Price = 1000
        };
        _mockTourService.Setup(x => x.UpdateTourAsync(It.IsAny<UpdateTourDto>()))
            .ReturnsAsync(new TourDto { Id = 1 });

        // Act
        var result = await _controller.Edit(1, updateDto, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateTourDto { Id = 1 };

        // Act
        var result = await _controller.Edit(2, updateDto, null);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithInvalidModelState_ReturnsView()
    {
        // Arrange
        var updateDto = new UpdateTourDto { Id = 1 };
        _controller.ModelState.AddModelError("TourName", "Required");

        // Act
        var result = await _controller.Edit(1, updateDto, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
    }

    [Fact]
    public async Task Delete_Get_WithValidId_ReturnsViewWithTour()
    {
        // Arrange
        var tourId = 1;
        var tour = new TourDto { Id = tourId, TourName = "Test Tour" };
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync(tour);

        // Act
        var result = await _controller.Delete(tourId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TourDto>(viewResult.Model);
        Assert.Equal(tourId, model.Id);
    }

    [Fact]
    public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(x => x.GetTourByIdAsync(tourId)).ReturnsAsync((TourDto?)null);

        // Act
        var result = await _controller.Delete(tourId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
    {
        // Arrange
        var tourId = 1;
        _mockTourService.Setup(x => x.DeleteTourAsync(tourId)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteConfirmed(tourId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task DeleteConfirmed_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(x => x.DeleteTourAsync(tourId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteConfirmed(tourId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Search_WithSearchTerm_ReturnsViewWithFilteredTours()
    {
        // Arrange
        var searchTerm = "Beach";
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Beach Tour" }
        };
        _mockTourService.Setup(x => x.SearchToursAsync(searchTerm)).ReturnsAsync(tours);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Search_WithEmptySearchTerm_ReturnsAllTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour 1" },
            new TourDto { Id = 2, TourName = "Tour 2" }
        };
        _mockTourService.Setup(x => x.SearchToursAsync(string.Empty)).ReturnsAsync(tours);

        // Act
        var result = await _controller.Search(null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Equal(2, ((List<TourDto>)model).Count);
    }

    [Fact]
    public async Task Search_WithException_ReturnsViewWithEmptyList()
    {
        // Arrange
        _mockTourService.Setup(x => x.SearchToursAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Search error"));

        // Act
        var result = await _controller.Search("test");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TourDto>>(viewResult.Model);
        Assert.Empty(model);
    }
}

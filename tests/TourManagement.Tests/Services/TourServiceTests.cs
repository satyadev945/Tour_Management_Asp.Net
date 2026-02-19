using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Application.DTOs;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Interfaces;
using Xunit;

namespace TourManagement.Tests.Services;

public class TourServiceTests
{
    private readonly Mock<ITourRepository> _mockTourRepository;
    private readonly Mock<ILogger<TourService>> _mockLogger;
    private readonly TourService _tourService;

    public TourServiceTests()
    {
        _mockTourRepository = new Mock<ITourRepository>();
        _mockLogger = new Mock<ILogger<TourService>>();
        _tourService = new TourService(_mockTourRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllToursAsync_ReturnsAllTours()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 },
            new Tour { Id = 2, TourName = "Tour 2", Place = "Place 2", Days = 7, Price = 1500 }
        };
        _mockTourRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tours);

        // Act
        var result = await _tourService.GetAllToursAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTourRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTourByIdAsync_ReturnsTour_WhenTourExists()
    {
        // Arrange
        var tour = new Tour { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 };
        _mockTourRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tour);

        // Act
        var result = await _tourService.GetTourByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Tour 1", result.TourName);
        _mockTourRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTourByIdAsync_ReturnsNull_WhenTourDoesNotExist()
    {
        // Arrange
        _mockTourRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tour?)null);

        // Act
        var result = await _tourService.GetTourByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockTourRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateTourAsync_CreatesTour_Successfully()
    {
        // Arrange
        var createDto = new CreateTourDto
        {
            TourName = "New Tour",
            Place = "New Place",
            Days = 5,
            Price = 1000,
            Locations = "Location 1, Location 2",
            TourInfo = "Tour information"
        };

        var createdTour = new Tour
        {
            Id = 1,
            TourName = createDto.TourName,
            Place = createDto.Place,
            Days = createDto.Days,
            Price = createDto.Price,
            Locations = createDto.Locations,
            TourInfo = createDto.TourInfo
        };

        _mockTourRepository.Setup(repo => repo.AddAsync(It.IsAny<Tour>())).ReturnsAsync(createdTour);

        // Act
        var result = await _tourService.CreateTourAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Tour", result.TourName);
        _mockTourRepository.Verify(repo => repo.AddAsync(It.IsAny<Tour>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTourAsync_DeletesTour_Successfully()
    {
        // Arrange
        _mockTourRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _tourService.DeleteTourAsync(1);

        // Assert
        Assert.True(result);
        _mockTourRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}

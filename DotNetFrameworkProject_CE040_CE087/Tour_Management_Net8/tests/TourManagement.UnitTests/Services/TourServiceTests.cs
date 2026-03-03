using AutoMapper;
using FluentAssertions;
using TourManagement.Domain.DTOs;

namespace TourManagement.UnitTests.Services;

public class TourServiceTests
{
    private readonly Mock<ITourRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TourService>> _mockLogger;
    private readonly TourService _service;

    public TourServiceTests()
    {
        _mockRepository = new Mock<ITourRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TourService>>();
        _service = new TourService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Tour 1", IsActive = true },
            new Tour { Id = 2, TourName = "Tour 2", IsActive = true }
        };
        var tourDtos = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour 1" },
            new TourDto { Id = 2, TourName = "Tour 2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);
        _mockMapper.Setup(m => m.Map<IEnumerable<TourDto>>(tours))
            .Returns(tourDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTour()
    {
        // Arrange
        var tour = new Tour { Id = 1, TourName = "Tour 1", IsActive = true };
        var tourDto = new TourDto { Id = 1, TourName = "Tour 1" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour))
            .Returns(tourDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.TourName.Should().Be("Tour 1");
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateTour()
    {
        // Arrange
        var createDto = new TourCreateDto { TourName = "New Tour" };
        var tour = new Tour { Id = 1, TourName = "New Tour", IsActive = true };
        var tourDto = new TourDto { Id = 1, TourName = "New Tour" };

        _mockMapper.Setup(m => m.Map<Tour>(createDto))
            .Returns(tour);
        _mockRepository.Setup(r => r.AddAsync(tour, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour))
            .Returns(tourDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.TourName.Should().Be("New Tour");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

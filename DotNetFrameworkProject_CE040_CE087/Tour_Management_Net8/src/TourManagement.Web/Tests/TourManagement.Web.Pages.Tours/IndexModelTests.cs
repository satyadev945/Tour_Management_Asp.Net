using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Pages.Tours;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Web.ViewModels;
using FluentAssertions;

namespace TourManagement.Web.Tests.Pages.Tours
{
    public class IndexModelTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly Mock<ILogger<IndexModel>> _mockLogger;
        private readonly IndexModel _indexModel;

        public IndexModelTests()
        {
            _mockTourService = new Mock<ITourService>();
            _mockLogger = new Mock<ILogger<IndexModel>>();
            _indexModel = new IndexModel(_mockTourService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var tourService = new Mock<ITourService>();
            var logger = new Mock<ILogger<IndexModel>>();

            // Act
            var model = new IndexModel(tourService.Object, logger.Object);

            // Assert
            model.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullTourService_ShouldThrowArgumentNullException()
        {
            // Arrange
            ITourService tourService = null!;
            var logger = new Mock<ILogger<IndexModel>>();

            // Act
            Action act = () => new IndexModel(tourService, logger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var tourService = new Mock<ITourService>();
            ILogger<IndexModel> logger = null!;

            // Act
            Action act = () => new IndexModel(tourService.Object, logger);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Tours_Property_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var model = new IndexModel(_mockTourService.Object, _mockLogger.Object);

            // Assert
            model.Tours.Should().NotBeNull();
            model.Tours.Should().BeEmpty();
        }

        [Fact]
        public void SearchTerm_Property_ShouldBeSettable()
        {
            // Arrange
            var model = new IndexModel(_mockTourService.Object, _mockLogger.Object);
            var searchTerm = "Paris";

            // Act
            model.SearchTerm = searchTerm;

            // Assert
            model.SearchTerm.Should().Be(searchTerm);
        }

        [Fact]
        public async Task OnGetAsync_WithNoSearchTerm_ShouldCallGetAllAsync()
        {
            // Arrange
            var tourDtos = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 }
            };
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _mockTourService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_WithSearchTerm_ShouldCallSearchAsync()
        {
            // Arrange
            var searchTerm = "Paris";
            _indexModel.SearchTerm = searchTerm;
            var tourDtos = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Paris Tour", Place = "Paris", Days = 5, Price = 1000 }
            };
            _mockTourService.Setup(s => s.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _mockTourService.Verify(s => s.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_WithEmptySearchTerm_ShouldCallGetAllAsync()
        {
            // Arrange
            _indexModel.SearchTerm = "";
            var tourDtos = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 }
            };
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _mockTourService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_WithWhitespaceSearchTerm_ShouldCallGetAllAsync()
        {
            // Arrange
            _indexModel.SearchTerm = "   ";
            var tourDtos = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 }
            };
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _mockTourService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_ShouldMapDtosToViewModels()
        {
            // Arrange
            var tourDtos = new List<TourDto>
            {
                new TourDto 
                { 
                    Id = 1, 
                    TourName = "Tour 1", 
                    Place = "Place 1", 
                    Days = 5, 
                    Price = 1000,
                    Locations = "Location 1",
                    TourInfo = "Info 1",
                    PictureFileName = "pic1.jpg"
                }
            };
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _indexModel.Tours.Should().HaveCount(1);
            var tour = _indexModel.Tours.First();
            tour.Id.Should().Be(1);
            tour.TourName.Should().Be("Tour 1");
            tour.Place.Should().Be("Place 1");
            tour.Days.Should().Be(5);
            tour.Price.Should().Be(1000);
        }

        [Fact]
        public async Task OnGetAsync_WithMultipleTours_ShouldMapAllTours()
        {
            // Arrange
            var tourDtos = new List<TourDto>
            {
                new TourDto { Id = 1, TourName = "Tour 1", Place = "Place 1", Days = 5, Price = 1000 },
                new TourDto { Id = 2, TourName = "Tour 2", Place = "Place 2", Days = 7, Price = 2000 },
                new TourDto { Id = 3, TourName = "Tour 3", Place = "Place 3", Days = 10, Price = 3000 }
            };
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _indexModel.Tours.Should().HaveCount(3);
        }

        [Fact]
        public async Task OnGetAsync_WithException_ShouldLogError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error loading tours")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task OnGetAsync_WithException_ShouldSetToursToEmptyList()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _indexModel.Tours.Should().NotBeNull();
            _indexModel.Tours.Should().BeEmpty();
        }

        [Fact]
        public async Task OnGetAsync_WithNoTours_ShouldReturnEmptyList()
        {
            // Arrange
            var tourDtos = new List<TourDto>();
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tourDtos);

            // Act
            await _indexModel.OnGetAsync();

            // Assert
            _indexModel.Tours.Should().BeEmpty();
        }

        [Fact]
        public async Task OnGetAsync_ShouldNotThrowException_WhenServiceReturnsNull()
        {
            // Arrange
            _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync((IEnumerable<TourDto>)null!);

            // Act
            Func<Task> act = async () => await _indexModel.OnGetAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public void IndexModel_ShouldInheritFromPageModel()
        {
            // Arrange & Act
            var model = new IndexModel(_mockTourService.Object, _mockLogger.Object);

            // Assert
            model.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.RazorPages.PageModel>();
        }
    }
}

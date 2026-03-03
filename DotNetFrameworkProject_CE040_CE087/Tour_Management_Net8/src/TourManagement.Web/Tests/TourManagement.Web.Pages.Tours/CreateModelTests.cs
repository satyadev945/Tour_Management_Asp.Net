using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using TourManagement.Web.Pages.Tours;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Web.ViewModels;
using FluentAssertions;

namespace TourManagement.Web.Tests.Pages.Tours
{
    public class CreateModelTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly Mock<ILogger<CreateModel>> _mockLogger;
        private readonly CreateModel _createModel;

        public CreateModelTests()
        {
            _mockTourService = new Mock<ITourService>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockLogger = new Mock<ILogger<CreateModel>>();
            
            _mockEnvironment.Setup(e => e.WebRootPath).Returns("/test/wwwroot");
            
            _createModel = new CreateModel(
                _mockTourService.Object,
                _mockEnvironment.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var tourService = new Mock<ITourService>();
            var environment = new Mock<IWebHostEnvironment>();
            var logger = new Mock<ILogger<CreateModel>>();

            // Act
            var model = new CreateModel(tourService.Object, environment.Object, logger.Object);

            // Assert
            model.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullTourService_ShouldThrowArgumentNullException()
        {
            // Arrange
            ITourService tourService = null!;
            var environment = new Mock<IWebHostEnvironment>();
            var logger = new Mock<ILogger<CreateModel>>();

            // Act
            Action act = () => new CreateModel(tourService, environment.Object, logger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullEnvironment_ShouldThrowArgumentNullException()
        {
            // Arrange
            var tourService = new Mock<ITourService>();
            IWebHostEnvironment environment = null!;
            var logger = new Mock<ILogger<CreateModel>>();

            // Act
            Action act = () => new CreateModel(tourService.Object, environment, logger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var tourService = new Mock<ITourService>();
            var environment = new Mock<IWebHostEnvironment>();
            ILogger<CreateModel> logger = null!;

            // Act
            Action act = () => new CreateModel(tourService.Object, environment.Object, logger);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Tour_Property_ShouldInitializeAsNewTourViewModel()
        {
            // Arrange & Act
            var model = new CreateModel(_mockTourService.Object, _mockEnvironment.Object, _mockLogger.Object);

            // Assert
            model.Tour.Should().NotBeNull();
            model.Tour.Should().BeOfType<TourViewModel>();
        }

        [Fact]
        public void OnGet_ShouldReturnPageResult()
        {
            // Arrange
            var model = new CreateModel(_mockTourService.Object, _mockEnvironment.Object, _mockLogger.Object);

            // Act
            var result = model.OnGet();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidModelState_ShouldReturnPageResult()
        {
            // Arrange
            _createModel.ModelState.AddModelError("TourName", "Required");

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_ShouldCallCreateAsync()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000,
                Locations = "Location 1",
                TourInfo = "Test Info"
            };

            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            _mockTourService.Verify(s => s.CreateAsync(It.IsAny<TourCreateDto>()), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000
            };

            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<RedirectToPageResult>();
            var redirectResult = result as RedirectToPageResult;
            redirectResult!.PageName.Should().Be("./Index");
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldLogError()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000
            };

            var exception = new Exception("Test exception");
            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .ThrowsAsync(exception);

            // Act
            await _createModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error creating tour")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldReturnPageResult()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000
            };

            var exception = new Exception("Test exception");
            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_WithException_ShouldAddModelError()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000
            };

            var exception = new Exception("Test exception");
            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .ThrowsAsync(exception);

            // Act
            await _createModel.OnPostAsync();

            // Assert
            _createModel.ModelState.Should().ContainKey(string.Empty);
            _createModel.ModelState[string.Empty]!.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task OnPostAsync_ShouldMapViewModelToDto()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000,
                Locations = "Location 1",
                TourInfo = "Test Info"
            };

            TourCreateDto? capturedDto = null;
            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .Callback<TourCreateDto>(dto => capturedDto = dto)
                .Returns(Task.CompletedTask);

            // Act
            await _createModel.OnPostAsync();

            // Assert
            capturedDto.Should().NotBeNull();
            capturedDto!.TourName.Should().Be("Test Tour");
            capturedDto.Place.Should().Be("Test Place");
            capturedDto.Days.Should().Be(5);
            capturedDto.Price.Should().Be(1000);
        }

        [Fact]
        public void CreateModel_ShouldInheritFromPageModel()
        {
            // Arrange & Act
            var model = new CreateModel(_mockTourService.Object, _mockEnvironment.Object, _mockLogger.Object);

            // Assert
            model.Should().BeAssignableTo<PageModel>();
        }

        [Fact]
        public async Task OnPostAsync_WithNullPictureFile_ShouldCreateTourWithoutFileName()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000,
                PictureFile = null
            };

            TourCreateDto? capturedDto = null;
            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .Callback<TourCreateDto>(dto => capturedDto = dto)
                .Returns(Task.CompletedTask);

            // Act
            await _createModel.OnPostAsync();

            // Assert
            capturedDto.Should().NotBeNull();
            capturedDto!.PictureFileName.Should().BeNull();
        }

        [Fact]
        public async Task OnPostAsync_WithValidData_ShouldSetSuccessMessage()
        {
            // Arrange
            _createModel.Tour = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 5,
                Price = 1000
            };

            _mockTourService.Setup(s => s.CreateAsync(It.IsAny<TourCreateDto>()))
                .Returns(Task.CompletedTask);

            // Mock TempData
            _createModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            // Act
            await _createModel.OnPostAsync();

            // Assert
            _createModel.TempData["SuccessMessage"].Should().Be("Tour created successfully!");
        }
    }
}

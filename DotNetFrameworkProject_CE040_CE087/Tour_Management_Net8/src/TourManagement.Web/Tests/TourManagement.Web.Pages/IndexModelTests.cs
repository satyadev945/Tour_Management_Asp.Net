using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Pages;
using FluentAssertions;

namespace TourManagement.Web.Tests.Pages
{
    public class IndexModelTests
    {
        private readonly Mock<ILogger<IndexModel>> _mockLogger;
        private readonly IndexModel _indexModel;

        public IndexModelTests()
        {
            _mockLogger = new Mock<ILogger<IndexModel>>();
            _indexModel = new IndexModel(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidLogger_ShouldCreateInstance()
        {
            // Arrange
            var logger = new Mock<ILogger<IndexModel>>();

            // Act
            var model = new IndexModel(logger.Object);

            // Assert
            model.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            ILogger<IndexModel> logger = null!;

            // Act
            Action act = () => new IndexModel(logger);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void OnGet_ShouldExecute_WithoutException()
        {
            // Arrange
            var model = new IndexModel(_mockLogger.Object);

            // Act
            Action act = () => model.OnGet();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void OnGet_ShouldLogInformation_WhenCalled()
        {
            // Arrange
            var model = new IndexModel(_mockLogger.Object);

            // Act
            model.OnGet();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Home page accessed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void OnGet_ShouldNotThrowException_WhenLoggerIsValid()
        {
            // Arrange
            var logger = new Mock<ILogger<IndexModel>>();
            var model = new IndexModel(logger.Object);

            // Act
            Action act = () => model.OnGet();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void IndexModel_ShouldInheritFromPageModel()
        {
            // Arrange & Act
            var model = new IndexModel(_mockLogger.Object);

            // Assert
            model.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.RazorPages.PageModel>();
        }

        [Fact]
        public void OnGet_CalledMultipleTimes_ShouldLogMultipleTimes()
        {
            // Arrange
            var model = new IndexModel(_mockLogger.Object);

            // Act
            model.OnGet();
            model.OnGet();
            model.OnGet();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Home page accessed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(3));
        }

        [Fact]
        public void OnGet_ShouldNotReturnValue()
        {
            // Arrange
            var model = new IndexModel(_mockLogger.Object);

            // Act
            model.OnGet();

            // Assert - Method is void, so just verify it completes
            model.Should().NotBeNull();
        }
    }
}

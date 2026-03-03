using Xunit;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TourManagement.Web.ViewModels;
using FluentAssertions;
using Moq;

namespace TourManagement.Web.Tests.ViewModels
{
    public class TourViewModelTests
    {
        [Fact]
        public void TourViewModel_ShouldInstantiate_WithDefaultValues()
        {
            // Arrange & Act
            var viewModel = new TourViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.TourName.Should().Be(string.Empty);
            viewModel.Place.Should().Be(string.Empty);
            viewModel.Locations.Should().Be(string.Empty);
            viewModel.TourInfo.Should().Be(string.Empty);
        }

        [Fact]
        public void Id_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedId = 1;

            // Act
            viewModel.Id = expectedId;

            // Assert
            viewModel.Id.Should().Be(expectedId);
        }

        [Fact]
        public void TourName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedTourName = "Paris Adventure";

            // Act
            viewModel.TourName = expectedTourName;

            // Assert
            viewModel.TourName.Should().Be(expectedTourName);
        }

        [Fact]
        public void Place_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedPlace = "Paris";

            // Act
            viewModel.Place = expectedPlace;

            // Assert
            viewModel.Place.Should().Be(expectedPlace);
        }

        [Fact]
        public void Days_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedDays = 7;

            // Act
            viewModel.Days = expectedDays;

            // Assert
            viewModel.Days.Should().Be(expectedDays);
        }

        [Fact]
        public void Price_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedPrice = 1500.50m;

            // Act
            viewModel.Price = expectedPrice;

            // Assert
            viewModel.Price.Should().Be(expectedPrice);
        }

        [Fact]
        public void Locations_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedLocations = "Paris, Lyon, Nice";

            // Act
            viewModel.Locations = expectedLocations;

            // Assert
            viewModel.Locations.Should().Be(expectedLocations);
        }

        [Fact]
        public void TourInfo_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedTourInfo = "Amazing tour of France";

            // Act
            viewModel.TourInfo = expectedTourInfo;

            // Assert
            viewModel.TourInfo.Should().Be(expectedTourInfo);
        }

        [Fact]
        public void PictureFileName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var expectedFileName = "tour-image.jpg";

            // Act
            viewModel.PictureFileName = expectedFileName;

            // Assert
            viewModel.PictureFileName.Should().Be(expectedFileName);
        }

        [Fact]
        public void PictureFile_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new TourViewModel();
            var mockFile = new Mock<IFormFile>();

            // Act
            viewModel.PictureFile = mockFile.Object;

            // Assert
            viewModel.PictureFile.Should().NotBeNull();
        }

        [Fact]
        public void TourName_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.TourName));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Place_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Place));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Days_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Days));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Days_ShouldHaveRangeAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Days));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Minimum.Should().Be(1);
            attribute.Maximum.Should().Be(365);
        }

        [Fact]
        public void Price_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Price));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Price_ShouldHaveRangeAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Price));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Minimum.Should().Be(0.01);
        }

        [Fact]
        public void Locations_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.Locations));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void TourInfo_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(TourViewModel).GetProperty(nameof(TourViewModel.TourInfo));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void TourViewModel_WithValidData_ShouldBeValid()
        {
            // Arrange
            var viewModel = new TourViewModel
            {
                Id = 1,
                TourName = "Paris Adventure",
                Place = "Paris",
                Days = 7,
                Price = 1500.50m,
                Locations = "Paris, Lyon, Nice",
                TourInfo = "Amazing tour of France"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void TourViewModel_WithAllProperties_ShouldSetCorrectly()
        {
            // Arrange & Act
            var viewModel = new TourViewModel
            {
                Id = 1,
                TourName = "Paris Adventure",
                Place = "Paris",
                Days = 7,
                Price = 1500.50m,
                Locations = "Paris, Lyon, Nice",
                TourInfo = "Amazing tour of France",
                PictureFileName = "tour-image.jpg"
            };

            // Assert
            viewModel.Id.Should().Be(1);
            viewModel.TourName.Should().Be("Paris Adventure");
            viewModel.Place.Should().Be("Paris");
            viewModel.Days.Should().Be(7);
            viewModel.Price.Should().Be(1500.50m);
            viewModel.Locations.Should().Be("Paris, Lyon, Nice");
            viewModel.TourInfo.Should().Be("Amazing tour of France");
            viewModel.PictureFileName.Should().Be("tour-image.jpg");
        }

        [Fact]
        public void PictureFileName_Property_ShouldBeNullable()
        {
            // Arrange
            var viewModel = new TourViewModel();

            // Act
            viewModel.PictureFileName = null;

            // Assert
            viewModel.PictureFileName.Should().BeNull();
        }

        [Fact]
        public void PictureFile_Property_ShouldBeNullable()
        {
            // Arrange
            var viewModel = new TourViewModel();

            // Act
            viewModel.PictureFile = null;

            // Assert
            viewModel.PictureFile.Should().BeNull();
        }

        [Fact]
        public void Days_WithValueInRange_ShouldBeValid()
        {
            // Arrange
            var viewModel = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 100,
                Price = 1000,
                Locations = "Location 1",
                TourInfo = "Test Info"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Days_WithValueOutOfRange_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 500,
                Price = 1000,
                Locations = "Location 1",
                TourInfo = "Test Info"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Price_WithValidValue_ShouldBeValid()
        {
            // Arrange
            var viewModel = new TourViewModel
            {
                TourName = "Test Tour",
                Place = "Test Place",
                Days = 7,
                Price = 100.50m,
                Locations = "Location 1",
                TourInfo = "Test Info"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void TourViewModel_WithMissingRequiredFields_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new TourViewModel
            {
                Id = 1
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
        }
    }
}

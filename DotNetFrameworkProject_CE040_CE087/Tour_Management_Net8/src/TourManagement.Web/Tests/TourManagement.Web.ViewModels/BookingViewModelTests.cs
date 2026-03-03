using Xunit;
using System.ComponentModel.DataAnnotations;
using TourManagement.Web.ViewModels;
using FluentAssertions;

namespace TourManagement.Web.Tests.ViewModels
{
    public class BookingViewModelTests
    {
        [Fact]
        public void BookingViewModel_ShouldInstantiate_WithDefaultValues()
        {
            // Arrange & Act
            var viewModel = new BookingViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be("Pending");
        }

        [Fact]
        public void Id_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedId = 1;

            // Act
            viewModel.Id = expectedId;

            // Assert
            viewModel.Id.Should().Be(expectedId);
        }

        [Fact]
        public void UserId_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedUserId = 10;

            // Act
            viewModel.UserId = expectedUserId;

            // Assert
            viewModel.UserId.Should().Be(expectedUserId);
        }

        [Fact]
        public void TourId_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedTourId = 5;

            // Act
            viewModel.TourId = expectedTourId;

            // Assert
            viewModel.TourId.Should().Be(expectedTourId);
        }

        [Fact]
        public void BookingDate_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedDate = new DateTime(2024, 12, 25);

            // Act
            viewModel.BookingDate = expectedDate;

            // Assert
            viewModel.BookingDate.Should().Be(expectedDate);
        }

        [Fact]
        public void NumberOfPeople_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedNumber = 4;

            // Act
            viewModel.NumberOfPeople = expectedNumber;

            // Assert
            viewModel.NumberOfPeople.Should().Be(expectedNumber);
        }

        [Fact]
        public void TotalAmount_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedAmount = 1500.50m;

            // Act
            viewModel.TotalAmount = expectedAmount;

            // Assert
            viewModel.TotalAmount.Should().Be(expectedAmount);
        }

        [Fact]
        public void Status_Property_ShouldDefaultToPending()
        {
            // Arrange & Act
            var viewModel = new BookingViewModel();

            // Assert
            viewModel.Status.Should().Be("Pending");
        }

        [Fact]
        public void Status_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedStatus = "Confirmed";

            // Act
            viewModel.Status = expectedStatus;

            // Assert
            viewModel.Status.Should().Be(expectedStatus);
        }

        [Fact]
        public void UserEmail_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedEmail = "user@example.com";

            // Act
            viewModel.UserEmail = expectedEmail;

            // Assert
            viewModel.UserEmail.Should().Be(expectedEmail);
        }

        [Fact]
        public void TourName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new BookingViewModel();
            var expectedTourName = "Paris Adventure";

            // Act
            viewModel.TourName = expectedTourName;

            // Assert
            viewModel.TourName.Should().Be(expectedTourName);
        }

        [Fact]
        public void UserId_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(BookingViewModel).GetProperty(nameof(BookingViewModel.UserId));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void TourId_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(BookingViewModel).GetProperty(nameof(BookingViewModel.TourId));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void BookingDate_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(BookingViewModel).GetProperty(nameof(BookingViewModel.BookingDate));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void NumberOfPeople_ShouldHaveRangeAttribute()
        {
            // Arrange
            var property = typeof(BookingViewModel).GetProperty(nameof(BookingViewModel.NumberOfPeople));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Minimum.Should().Be(1);
            attribute.Maximum.Should().Be(100);
        }

        [Fact]
        public void TotalAmount_ShouldHaveRangeAttribute()
        {
            // Arrange
            var property = typeof(BookingViewModel).GetProperty(nameof(BookingViewModel.TotalAmount));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Minimum.Should().Be(0.01);
        }

        [Fact]
        public void BookingViewModel_WithAllProperties_ShouldSetCorrectly()
        {
            // Arrange & Act
            var viewModel = new BookingViewModel
            {
                Id = 1,
                UserId = 10,
                TourId = 5,
                BookingDate = new DateTime(2024, 12, 25),
                NumberOfPeople = 4,
                TotalAmount = 1500.50m,
                Status = "Confirmed",
                UserEmail = "user@example.com",
                TourName = "Paris Adventure"
            };

            // Assert
            viewModel.Id.Should().Be(1);
            viewModel.UserId.Should().Be(10);
            viewModel.TourId.Should().Be(5);
            viewModel.BookingDate.Should().Be(new DateTime(2024, 12, 25));
            viewModel.NumberOfPeople.Should().Be(4);
            viewModel.TotalAmount.Should().Be(1500.50m);
            viewModel.Status.Should().Be("Confirmed");
            viewModel.UserEmail.Should().Be("user@example.com");
            viewModel.TourName.Should().Be("Paris Adventure");
        }

        [Fact]
        public void UserEmail_Property_ShouldBeNullable()
        {
            // Arrange
            var viewModel = new BookingViewModel();

            // Act
            viewModel.UserEmail = null;

            // Assert
            viewModel.UserEmail.Should().BeNull();
        }

        [Fact]
        public void TourName_Property_ShouldBeNullable()
        {
            // Arrange
            var viewModel = new BookingViewModel();

            // Act
            viewModel.TourName = null;

            // Assert
            viewModel.TourName.Should().BeNull();
        }

        [Fact]
        public void NumberOfPeople_WithValidRange_ShouldBeValid()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                UserId = 1,
                TourId = 1,
                BookingDate = DateTime.Now,
                NumberOfPeople = 50,
                TotalAmount = 1000
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void TotalAmount_WithValidValue_ShouldBeValid()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                UserId = 1,
                TourId = 1,
                BookingDate = DateTime.Now,
                NumberOfPeople = 2,
                TotalAmount = 100.50m
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }
    }
}

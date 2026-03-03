using Xunit;
using System.ComponentModel.DataAnnotations;
using TourManagement.Web.ViewModels;
using FluentAssertions;

namespace TourManagement.Web.Tests.ViewModels
{
    public class UserViewModelTests
    {
        [Fact]
        public void UserViewModel_ShouldInstantiate_WithDefaultValues()
        {
            // Arrange & Act
            var viewModel = new UserViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.Email.Should().Be(string.Empty);
        }

        [Fact]
        public void Id_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var expectedId = 1;

            // Act
            viewModel.Id = expectedId;

            // Assert
            viewModel.Id.Should().Be(expectedId);
        }

        [Fact]
        public void Email_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var expectedEmail = "test@example.com";

            // Act
            viewModel.Email = expectedEmail;

            // Assert
            viewModel.Email.Should().Be(expectedEmail);
        }

        [Fact]
        public void FirstName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var expectedFirstName = "John";

            // Act
            viewModel.FirstName = expectedFirstName;

            // Assert
            viewModel.FirstName.Should().Be(expectedFirstName);
        }

        [Fact]
        public void LastName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var expectedLastName = "Doe";

            // Act
            viewModel.LastName = expectedLastName;

            // Assert
            viewModel.LastName.Should().Be(expectedLastName);
        }

        [Fact]
        public void PhoneNumber_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var expectedPhoneNumber = "1234567890";

            // Act
            viewModel.PhoneNumber = expectedPhoneNumber;

            // Assert
            viewModel.PhoneNumber.Should().Be(expectedPhoneNumber);
        }

        [Fact]
        public void Email_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(UserViewModel).GetProperty(nameof(UserViewModel.Email));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Email_ShouldHaveEmailAddressAttribute()
        {
            // Arrange
            var property = typeof(UserViewModel).GetProperty(nameof(UserViewModel.Email));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(EmailAddressAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<EmailAddressAttribute>();
        }

        [Fact]
        public void PhoneNumber_ShouldHavePhoneAttribute()
        {
            // Arrange
            var property = typeof(UserViewModel).GetProperty(nameof(UserViewModel.PhoneNumber));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(PhoneAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<PhoneAttribute>();
        }

        [Fact]
        public void UserViewModel_WithValidData_ShouldBeValid()
        {
            // Arrange
            var viewModel = new UserViewModel
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }
    }

    public class UserRegisterViewModelTests
    {
        [Fact]
        public void UserRegisterViewModel_ShouldInstantiate_WithDefaultValues()
        {
            // Arrange & Act
            var viewModel = new UserRegisterViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.Email.Should().Be(string.Empty);
            viewModel.Password.Should().Be(string.Empty);
            viewModel.ConfirmPassword.Should().Be(string.Empty);
        }

        [Fact]
        public void Email_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedEmail = "test@example.com";

            // Act
            viewModel.Email = expectedEmail;

            // Assert
            viewModel.Email.Should().Be(expectedEmail);
        }

        [Fact]
        public void Password_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedPassword = "Password123!";

            // Act
            viewModel.Password = expectedPassword;

            // Assert
            viewModel.Password.Should().Be(expectedPassword);
        }

        [Fact]
        public void ConfirmPassword_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedConfirmPassword = "Password123!";

            // Act
            viewModel.ConfirmPassword = expectedConfirmPassword;

            // Assert
            viewModel.ConfirmPassword.Should().Be(expectedConfirmPassword);
        }

        [Fact]
        public void FirstName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedFirstName = "John";

            // Act
            viewModel.FirstName = expectedFirstName;

            // Assert
            viewModel.FirstName.Should().Be(expectedFirstName);
        }

        [Fact]
        public void LastName_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedLastName = "Doe";

            // Act
            viewModel.LastName = expectedLastName;

            // Assert
            viewModel.LastName.Should().Be(expectedLastName);
        }

        [Fact]
        public void PhoneNumber_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel();
            var expectedPhoneNumber = "1234567890";

            // Act
            viewModel.PhoneNumber = expectedPhoneNumber;

            // Assert
            viewModel.PhoneNumber.Should().Be(expectedPhoneNumber);
        }

        [Fact]
        public void Email_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(UserRegisterViewModel).GetProperty(nameof(UserRegisterViewModel.Email));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Password_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(UserRegisterViewModel).GetProperty(nameof(UserRegisterViewModel.Password));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Password_ShouldHaveMinLengthAttribute()
        {
            // Arrange
            var property = typeof(UserRegisterViewModel).GetProperty(nameof(UserRegisterViewModel.Password));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault() as MinLengthAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Length.Should().Be(6);
        }

        [Fact]
        public void ConfirmPassword_ShouldHaveCompareAttribute()
        {
            // Arrange
            var property = typeof(UserRegisterViewModel).GetProperty(nameof(UserRegisterViewModel.ConfirmPassword));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(CompareAttribute), false).FirstOrDefault() as CompareAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.OtherProperty.Should().Be("Password");
        }

        [Fact]
        public void UserRegisterViewModel_WithMatchingPasswords_ShouldBeValid()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void UserRegisterViewModel_WithNonMatchingPasswords_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword",
                FirstName = "John",
                LastName = "Doe"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("Passwords do not match"));
        }

        [Fact]
        public void UserRegisterViewModel_WithShortPassword_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new UserRegisterViewModel
            {
                Email = "test@example.com",
                Password = "Pass",
                ConfirmPassword = "Pass",
                FirstName = "John",
                LastName = "Doe"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
        }
    }

    public class UserLoginViewModelTests
    {
        [Fact]
        public void UserLoginViewModel_ShouldInstantiate_WithDefaultValues()
        {
            // Arrange & Act
            var viewModel = new UserLoginViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.Email.Should().Be(string.Empty);
            viewModel.Password.Should().Be(string.Empty);
        }

        [Fact]
        public void Email_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserLoginViewModel();
            var expectedEmail = "test@example.com";

            // Act
            viewModel.Email = expectedEmail;

            // Assert
            viewModel.Email.Should().Be(expectedEmail);
        }

        [Fact]
        public void Password_Property_ShouldBeSettable()
        {
            // Arrange
            var viewModel = new UserLoginViewModel();
            var expectedPassword = "Password123!";

            // Act
            viewModel.Password = expectedPassword;

            // Assert
            viewModel.Password.Should().Be(expectedPassword);
        }

        [Fact]
        public void Email_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(UserLoginViewModel).GetProperty(nameof(UserLoginViewModel.Email));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void Email_ShouldHaveEmailAddressAttribute()
        {
            // Arrange
            var property = typeof(UserLoginViewModel).GetProperty(nameof(UserLoginViewModel.Email));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(EmailAddressAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<EmailAddressAttribute>();
        }

        [Fact]
        public void Password_ShouldHaveRequiredAttribute()
        {
            // Arrange
            var property = typeof(UserLoginViewModel).GetProperty(nameof(UserLoginViewModel.Password));

            // Act
            var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            attribute.Should().NotBeNull();
            attribute.Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void UserLoginViewModel_WithValidData_ShouldBeValid()
        {
            // Arrange
            var viewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void UserLoginViewModel_WithInvalidEmail_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new UserLoginViewModel
            {
                Email = "invalid-email",
                Password = "Password123!"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void UserLoginViewModel_WithEmptyPassword_ShouldBeInvalid()
        {
            // Arrange
            var viewModel = new UserLoginViewModel
            {
                Email = "test@example.com",
                Password = ""
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}

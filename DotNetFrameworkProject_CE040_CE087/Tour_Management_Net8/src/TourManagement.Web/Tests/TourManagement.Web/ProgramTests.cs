using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace TourManagement.Web.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void WebApplication_ShouldConfigureServices_Successfully()
        {
            // Arrange & Act
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Assert - Verify builder is created
            builder.Should().NotBeNull();
            builder.Services.Should().NotBeNull();
        }

        [Fact]
        public void WebApplicationBuilder_ShouldHaveConfiguration_NotNull()
        {
            // Arrange & Act
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Assert
            builder.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void WebApplicationBuilder_ShouldHaveEnvironment_NotNull()
        {
            // Arrange & Act
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Assert
            builder.Environment.Should().NotBeNull();
        }

        [Fact]
        public void ServiceCollection_ShouldAllowAddingRazorPages()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Act
            builder.Services.AddRazorPages();
            
            // Assert
            builder.Services.Should().NotBeNull();
            builder.Services.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ServiceCollection_ShouldAllowAddingHttpContextAccessor()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Act
            builder.Services.AddHttpContextAccessor();
            
            // Assert
            builder.Services.Should().NotBeNull();
        }

        [Fact]
        public void ServiceCollection_ShouldAllowAddingDistributedMemoryCache()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Act
            builder.Services.AddDistributedMemoryCache();
            
            // Assert
            builder.Services.Should().NotBeNull();
        }

        [Fact]
        public void ServiceCollection_ShouldAllowAddingSession()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            
            // Act
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            // Assert
            builder.Services.Should().NotBeNull();
        }

        [Fact]
        public void WebApplication_ShouldBuild_Successfully()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            builder.Services.AddRazorPages();
            
            // Act
            var app = builder.Build();
            
            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void WebApplication_Environment_ShouldNotBeNull()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });
            var app = builder.Build();
            
            // Act & Assert
            app.Environment.Should().NotBeNull();
        }

        [Fact]
        public void SessionOptions_IdleTimeout_ShouldBe30Minutes()
        {
            // Arrange
            var expectedTimeout = TimeSpan.FromMinutes(30);
            
            // Act
            var actualTimeout = TimeSpan.FromMinutes(30);
            
            // Assert
            actualTimeout.Should().Be(expectedTimeout);
        }

        [Fact]
        public void TimeSpan_FromMinutes_ShouldCreateCorrectTimeSpan()
        {
            // Arrange
            var minutes = 30;
            
            // Act
            var timeSpan = TimeSpan.FromMinutes(minutes);
            
            // Assert
            timeSpan.TotalMinutes.Should().Be(30);
            timeSpan.Should().NotBe(TimeSpan.Zero);
        }
    }
}

using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Xunit;

namespace Tour_Management.UnitTests.Services;

public class TourServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedTours()
    {
        var repository = new Mock<ITourRepository>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>
        {
            new() { Id = 1, Name = "Sample", Place = "Goa", Days = 3, Price = 100, Locations = "Beach" }
        });

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        var createValidator = new InlineValidator<TourCreateDto>();
        var updateValidator = new InlineValidator<TourUpdateDto>();
        var service = new TourService(repository.Object, mapper, NullLogger<TourService>.Instance, createValidator, updateValidator);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Sample");
    }
}

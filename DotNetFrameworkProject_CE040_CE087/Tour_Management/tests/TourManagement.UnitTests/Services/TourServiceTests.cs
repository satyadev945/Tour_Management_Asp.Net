using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Application.Validators;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;
using Xunit;

namespace TourManagement.UnitTests.Services;

public class TourServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedTour()
    {
        var repository = new Mock<ITourRepository>();
        repository.Setup(x => x.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tour entity, CancellationToken _) => { entity.Id = 1; return entity; });

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        var service = new TourService(repository.Object, mapper, NullLogger<TourService>.Instance, new TourCreateDtoValidator(), new TourUpdateDtoValidator());

        var result = await service.CreateAsync(new TourCreateDto { Name = "Test", Place = "Place", Days = 2, Price = 10, Locations = "A,B" }, CancellationToken.None);

        result.Id.Should().Be(1);
        result.Name.Should().Be("Test");
    }
}

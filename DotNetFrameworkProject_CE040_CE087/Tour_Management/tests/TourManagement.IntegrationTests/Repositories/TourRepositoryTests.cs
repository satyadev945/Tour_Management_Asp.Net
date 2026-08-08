using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;
using Xunit;

namespace TourManagement.IntegrationTests.Repositories;

public class TourRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistTour()
    {
        var options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new TourManagementDbContext(options);
        var repository = new TourRepository(context);

        var result = await repository.AddAsync(new Tour { Name = "Integration", Place = "Test", Days = 3, Price = 100, Locations = "Loc", CreatedBy = "test" }, CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
        (await context.Tours.CountAsync()).Should().Be(1);
    }
}

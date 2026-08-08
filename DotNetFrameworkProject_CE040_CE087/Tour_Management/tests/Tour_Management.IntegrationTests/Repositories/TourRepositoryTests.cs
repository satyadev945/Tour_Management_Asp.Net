using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Tour_Management.Domain.Entities;
using Tour_Management.Infrastructure.Data;
using Tour_Management.Infrastructure.Repositories;
using Xunit;

namespace Tour_Management.IntegrationTests.Repositories;

public class TourRepositoryTests
{
    [Fact]
    public async Task AddAsync_PersistsTour()
    {
        var options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new TourManagementDbContext(options);
        var repository = new TourRepository(context, NullLogger<TourRepository>.Instance);

        var created = await repository.AddAsync(new Tour
        {
            Name = "Kerala Escape",
            Place = "Kerala",
            Days = 5,
            Price = 250,
            Locations = "Munnar"
        }, CancellationToken.None);

        created.Id.Should().BeGreaterThan(0);
        (await context.Tours.CountAsync()).Should().Be(1);
    }
}

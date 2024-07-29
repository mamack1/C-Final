using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using C_DiscApp.Data;

namespace C_DiscAppUnitTests
{
    public class DiscServiceTests
    {
        private DiscContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DiscContext>()
                .UseInMemoryDatabase(databaseName: "DiscTestDb")
                .Options;

            var dbContext = new DiscContext(options);
            dbContext.Database.EnsureDeleted(); // Ensure a clean state for each test
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [Fact]
        public async Task GetAllDiscsAsync_ReturnsDiscsForSpecificUser()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            dbContext.Discs.AddRange(
                new Disc { DiscID = 1, Name = "Disc 1", UserId = "userId1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" },
                new Disc { DiscID = 2, Name = "Disc 2", UserId = "userId1", Brand = "Brand B", Color = "Blue", Description = "Description 2", ImageUrl = "http://example.com/disc2.png", Type = "Type B" },
                new Disc { DiscID = 3, Name = "Disc 3", UserId = "userId2", Brand = "Brand C", Color = "Green", Description = "Description 3", ImageUrl = "http://example.com/disc3.png", Type = "Type C" }
            );
            await dbContext.SaveChangesAsync();

            var discService = new DiscService(dbContext);

            // Act
            var result = await discService.GetAllDiscsAsync("userId1");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Name == "Disc 1");
            Assert.Contains(result, d => d.Name == "Disc 2");
            Assert.DoesNotContain(result, d => d.Name == "Disc 3"); // Belongs to another user
        }

        [Fact]
        public async Task GetDiscByIdAsync_ValidId_ReturnsDisc()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            dbContext.Discs.AddRange(
                new Disc { DiscID = 1, Name = "Disc 1", UserId = "userId1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" },
                new Disc { DiscID = 2, Name = "Disc 2", UserId = "userId1", Brand = "Brand B", Color = "Blue", Description = "Description 2", ImageUrl = "http://example.com/disc2.png", Type = "Type B" }
            );
            await dbContext.SaveChangesAsync();

            var discService = new DiscService(dbContext);

            // Act
            var result = await discService.GetDiscByIdAsync(1, "userId1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Disc 1", result.Name);
        }

        [Fact]
        public async Task AddDiscAsync_ValidDisc_AddsDisc()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var discService = new DiscService(dbContext);
            var disc = new Disc { Name = "Test Disc", UserId = "userId1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" };

            // Act
            await discService.AddDiscAsync(disc);

            // Assert
            var addedDisc = await dbContext.Discs.FirstOrDefaultAsync(d => d.Name == "Test Disc");
            Assert.NotNull(addedDisc);
        }

        [Fact]
        public async Task UpdateDiscAsync_ValidDisc_UpdatesDisc()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            dbContext.Discs.Add(new Disc { DiscID = 1, Name = "Disc 1", UserId = "userId1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" });
            await dbContext.SaveChangesAsync();

            var discService = new DiscService(dbContext);
            var disc = await dbContext.Discs.FirstAsync();
            disc.Name = "Updated Disc";

            // Act
            await discService.UpdateDiscAsync(disc);

            // Assert
            var updatedDisc = await dbContext.Discs.FirstAsync();
            Assert.Equal("Updated Disc", updatedDisc.Name);
        }

        [Fact]
        public async Task DeleteDiscAsync_ValidId_DeletesDisc()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            dbContext.Discs.Add(new Disc { DiscID = 1, Name = "Disc 1", UserId = "userId1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" });
            await dbContext.SaveChangesAsync();

            var discService = new DiscService(dbContext);

            // Act
            await discService.DeleteDiscAsync(1, "userId1");

            // Assert
            var deletedDisc = await dbContext.Discs.FirstOrDefaultAsync(d => d.DiscID == 1);
            Assert.Null(deletedDisc);
        }
    }
}
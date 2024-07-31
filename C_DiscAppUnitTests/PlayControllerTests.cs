using C_DiscApp.Controllers;
using C_DiscApp.Data;
using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace C_DiscAppUnitTests
{
    public class PlayControllerTests
    {
        private DiscContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DiscContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new DiscContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var userStore = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void Start_ReturnsPlayView_WithCorrectViewBagValues()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mockDiscService = new Mock<IDiscService>();
            var mockLogger = new Mock<ILogger<InventoryController>>();
            var mockUserManager = GetMockUserManager();
            var controller = new PlayController(context, mockDiscService.Object, mockLogger.Object, mockUserManager.Object);

            // Act
            var result = controller.Start("Course 1", "Address 1", 5.0, 18) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Play", result.ViewName);
            Assert.Equal("Course 1", result.ViewData["CourseName"]);
            Assert.Equal("Address 1", result.ViewData["CourseAddress"]);
            Assert.Equal(5.0, result.ViewData["CourseDistance"]);
            Assert.Equal(18, result.ViewData["NumberOfHoles"]);
            Assert.Equal(1, result.ViewData["CurrentHole"]);
        }

        [Fact]
        public async Task SaveGame_RedirectsToHistoryIndex_WithValidData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mockDiscService = new Mock<IDiscService>();
            var mockLogger = new Mock<ILogger<InventoryController>>();
            var mockUserManager = GetMockUserManager();
            var controller = new PlayController(context, mockDiscService.Object, mockLogger.Object, mockUserManager.Object);
            var pars = new List<int> { 3, 3, 3 };
            var throws = new List<int> { 4, 3, 3 };

            var user = new User { Id = "user1" };
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(user.Id);

            // Act
            var result = await controller.SaveGame(3, "Course 1", pars, throws) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("History", result.ControllerName);

            var savedGame = context.GameHistories.FirstOrDefault();
            Assert.NotNull(savedGame);
            Assert.Equal("Course 1", savedGame.CourseName);
            Assert.Equal(3, savedGame.NumberOfHoles);
            Assert.Equal(9, savedGame.TotalParThrows);
            Assert.Equal(10, savedGame.TotalThrows);
            Assert.Equal("user1", savedGame.UserId);
        }

        [Fact]
        public async Task SaveGame_ReturnsPlayView_WithModelError_WhenCourseNameIsEmpty()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mockDiscService = new Mock<IDiscService>();
            var mockLogger = new Mock<ILogger<InventoryController>>();
            var mockUserManager = GetMockUserManager();
            var controller = new PlayController(context, mockDiscService.Object, mockLogger.Object, mockUserManager.Object);
            var pars = new List<int> { 3, 3, 3 };
            var throws = new List<int> { 4, 3, 3 };

            // Act
            var result = await controller.SaveGame(3, "", pars, throws) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Play", result.ViewName);
            Assert.True(controller.ModelState.ContainsKey(""));
            Assert.Equal("Course name is required.", controller.ModelState[""].Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task SaveGame_ReturnsPlayView_WithModelError_WhenParsThrowsCountMismatch()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mockDiscService = new Mock<IDiscService>();
            var mockLogger = new Mock<ILogger<InventoryController>>();
            var mockUserManager = GetMockUserManager();
            var controller = new PlayController(context, mockDiscService.Object, mockLogger.Object, mockUserManager.Object);
            var pars = new List<int> { 3, 3, 3 };
            var throws = new List<int> { 4, 3 };

            // Act
            var result = await controller.SaveGame(3, "Course 1", pars, throws) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Play", result.ViewName);
            Assert.True(controller.ModelState.ContainsKey(""));
            Assert.Equal("Mismatch between number of holes and provided scores.", controller.ModelState[""].Errors.First().ErrorMessage);
        }
    }
}

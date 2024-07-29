using C_DiscApp.Controllers;
using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Security.Claims;

namespace C_DiscAppUnitTests
{
    public class InventoryControllerTests
    {
        private Mock<UserManager<User>> mockUserManager;
        private Mock<IDiscService> mockDiscService;
        private List<User> userList;

        public InventoryControllerTests()
        {
            userList = new List<User>();
            mockUserManager = MockHelpers.MockUserManager(userList);
            mockDiscService = new Mock<IDiscService>();
        }

        [Fact]
        public async Task Index_ReturnsViewWithDiscs()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";
            var discs = new List<Disc> { new Disc { DiscID = 1, Name = "Disc 1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" } };

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.GetAllDiscsAsync(userId)).ReturnsAsync(discs);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(discs, result.Model);
        }

        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_Post_RedirectsToIndexOnSuccess()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";
            var disc = new Disc { DiscID = 1, Name = "Disc 1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" };

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.AddDiscAsync(disc)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Create(disc) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(controller.Index), result.ActionName);
        }

        [Fact]
        public async Task Edit_ReturnsViewWithDisc()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";
            var disc = new Disc { DiscID = 1, Name = "Disc 1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" };

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.GetDiscByIdAsync(1, userId)).ReturnsAsync(disc);

            // Act
            var result = await controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(disc, result.Model);
        }

        [Fact]
        public async Task Edit_Post_RedirectsToIndexOnSuccess()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";
            var disc = new Disc { DiscID = 1, Name = "Disc 1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" };

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.UpdateDiscAsync(disc)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Edit(1, disc) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(controller.Index), result.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsViewWithDisc()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";
            var disc = new Disc { DiscID = 1, Name = "Disc 1", Brand = "Brand A", Color = "Red", Description = "Description 1", ImageUrl = "http://example.com/disc1.png", Type = "Type A" };

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.GetDiscByIdAsync(1, userId)).ReturnsAsync(disc);

            // Act
            var result = await controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(disc, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndexOnSuccess()
        {
            // Arrange
            var controller = new InventoryController(mockDiscService.Object, mockUserManager.Object);
            var userId = "userId";

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            mockDiscService.Setup(ds => ds.DeleteDiscAsync(1, userId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(controller.Index), result.ActionName);
        }
    }
}
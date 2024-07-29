using C_DiscApp.Controllers;
using C_DiscApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using C_DiscApp.ViewModels;

namespace C_DiscAppUnitTests
{
    public class AccountControllerTests
    {
        private Mock<UserManager<User>> mockUserManager;
        private Mock<SignInManager<User>> mockSignInManager;
        private List<User> userList;

        public AccountControllerTests()
        {
            userList = new List<User>();
            mockUserManager = MockHelpers.MockUserManager(userList);
            mockSignInManager = MockHelpers.MockSignInManager(mockUserManager.Object);
        }

        [Fact]
        public void Register_Get_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Register_Post_ReturnsViewIfInvalidModel()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);
            controller.ModelState.AddModelError("Error", "ModelError");

            var model = new RegisterViewModel { Username = "testuser", Password = "Password123!" };

            // Act
            var result = await controller.Register(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Register_Post_RedirectsOnSuccess()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);
            var model = new RegisterViewModel { Username = "testuser", Password = "Password123!" };

            mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public void LogIn_Get_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = controller.LogIn() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task LogIn_Post_ReturnsViewIfInvalidModel()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);
            controller.ModelState.AddModelError("Error", "ModelError");

            var model = new LoginViewModel { Username = "testuser", Password = "Password123!" };

            // Act
            var result = await controller.LogIn(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task LogIn_Post_RedirectsOnSuccess()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);
            var model = new LoginViewModel { Username = "testuser", Password = "Password123!" };

            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await controller.LogIn(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public async Task LogOut_RedirectsToHomeIndex()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = await controller.LogOut() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            mockSignInManager.Verify(s => s.SignOutAsync(), Times.Once);
        }
    }
}
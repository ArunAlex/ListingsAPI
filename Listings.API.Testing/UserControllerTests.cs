using FluentAssertions;
using Listings.API.Controllers;
using Listings.Domain.Models;
using Listings.Domain.Interfaces;
using Listings.Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Listings.API.Testing
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserRepository> _mockUserRepository = new();

        public UserControllerTests()
        {
            _controller = new UserController(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(c => c.GetUserAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                    .Which.Value.Should().Be("User not found");
        }

        [Fact]
        public async Task GetUser_ShouldReturnOk_WhenUserExist()
        {
            // Arrange
            var expectedUser = new User() {
                Id = 1, 
                Username="user1", 
                Email="user1@email.com", 
                CreatedAt=DateTime.Now,
                PasswordHash = "XXXXXXX"
            };
            _mockUserRepository.Setup(c => c.GetUserAsync(1))
                .ReturnsAsync(expectedUser);

            // Actdot
            var result = await _controller.GetUser(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenUserIsValid()
        {
            // Arrange
            var newUser = new User()
            {
                Id = 2,
                Username = "user2",
                Email = "user2@email.com",
                CreatedAt = DateTime.Now,
                PasswordHash = "YYYYYYY"
            };

            var createUserRequest = new CreateUserRequest()
            {
                Username = newUser.Username,
                Email = newUser.Email,  
                PasswordHash = newUser.PasswordHash
            };

            _mockUserRepository.Setup(c => c.CreateUserAsync(createUserRequest)).ReturnsAsync(newUser);

            // Act
            var result = await _controller.CreateUser(createUserRequest);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().BeEquivalentTo(newUser);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var updateUserRequest = new UpdateUserRequest()
            {
                Username = "user2",
                Email = "test@email.com",  
                PasswordHash = "XXXXXXX"
            };

            _mockUserRepository.Setup(c => c.UpdateUserAsync(1, updateUserRequest)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(1, updateUserRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidUserRequest = new CreateUserRequest()
            {
                // Set properties to invalid values
                Email = "invalidemail"
            };

            _controller.ModelState.AddModelError("UserName", "Username field is required");
            _controller.ModelState.AddModelError("Email", "Email is invalid");

            // Act
            var result = await _controller.CreateUser(invalidUserRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(_controller.ModelState.Select(x=> new 
                    {
                        x.Key,
                        Value = x.Value?.Errors.Select(x=>x.ErrorMessage)
                    }).ToList());
        }     

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenUserDoesNotExists()
        {
            // Arrange
            var updateUserRequest = new UpdateUserRequest();

            _mockUserRepository.Setup(c => c.UpdateUserAsync(1, updateUserRequest)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(1, updateUserRequest);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("User not found");
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(c => c.GetUserAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("User not found");
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(c => c.GetUserAsync(userId))
                .ReturnsAsync(new User() { Id = userId });
            _mockUserRepository.Setup(c => c.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        } 
    }
}
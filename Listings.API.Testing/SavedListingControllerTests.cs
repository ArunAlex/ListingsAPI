using FluentAssertions;
using Listings.API.Controllers;
using Listings.Domain.Models;
using Listings.Domain.Requests;
using Listings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Listings.API.Testing
{
    public class SavedListingControllerTests
    {
        private readonly SavedListingController _controller;
        private readonly Mock<ISavedListingRepository> _mockSavedListingRepository = new();

        public SavedListingControllerTests()
        {
            _controller = new SavedListingController(_mockSavedListingRepository.Object);
        }

        [Fact]
        public async void GetSavedListings_ReturnsOkResult_WhenListingExists()
        {
            // Arrange
            var listing1 = new Listings.Domain.Models.Listing()
            {
                Id = 1,
                Address = "Address",
                Suburb = "Suburb",
                State = "State",
                Postcode = 2154
            };

            var listing2 = new Listings.Domain.Models.Listing()
            {
                Id = 2,
                Address = "Address",
                Suburb = "Suburb",
                State = "State",
                Postcode = 2154
            };

            var user = new User()
            {
                Id = 1,
                Username = "user1",
                Email = "user1@email.com",
                CreatedAt = DateTime.Now,
                PasswordHash = "XXXXXXX"
            };

            var expectedSavedListings = new List<SavedListing>()
            {
                new SavedListing { UserId = 1, User = user, ListingId = 1, Listing = listing1, SavedAt = DateTime.Now },
                new SavedListing { UserId = 1, User = user, ListingId = 2, Listing = listing2, SavedAt = DateTime.Now }
            };

            _mockSavedListingRepository.Setup(c => c.GetSavedListingsByUserAsync(1))
                .ReturnsAsync(expectedSavedListings);

            // Act
            var result = await _controller.GetSavedListings(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeEquivalentTo(expectedSavedListings);
        }

        [Fact]
        public async void GetSavedListings_ReturnsNotFoundResult_WhenListingDoesNotExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.GetSavedListingsByUserAsync(1))
                .ReturnsAsync(Enumerable.Empty<SavedListing>);

            // Act
            var result = await _controller.GetSavedListings(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("No Listings for user id 1");
        }


        [Fact]
        public async Task CreateSavedListing_ShouldReturnBadRequest_WhenInputIsInvalid()
        {
            // Act
            var result = await _controller.CreateSavedListing(new CreateSavedListingRequest() { UserId = -1, ListingId = 1});

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("User Id is invalid");

            // Act
            result = await _controller.CreateSavedListing(new CreateSavedListingRequest() { UserId = 1, ListingId = -1});
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Listing Id is invalid");
        }

        [Fact]
        public async Task CreateSavedListing_ShouldReturnCreated_WhenInputIsValid()
        {
            // Arrange
            var expectedSavedListing = new SavedListing()
            {
                UserId = 1,
                ListingId = 1,
            };
            _mockSavedListingRepository.Setup(c => c.CreateSavedListingAsync(1,1))
                .ReturnsAsync(expectedSavedListing);

            // Act
            var result = await _controller.CreateSavedListing(new CreateSavedListingRequest() { UserId = 1, ListingId = 1});

            // Assert
            result.Should().BeOfType<CreatedResult>()
                    .Which.Value.Should().BeEquivalentTo(expectedSavedListing);
        }

        [Fact]
        public async Task UpdateSavedListing_ShouldReturnBadRequest_WhenInputInvalid()
        {
            // Arrange

            // Act
            var result = await _controller.UpdateSavedListing(-1, new UpdateSavedListingRequest() { OldListingId = 1, NewListingId = 1});

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().Be("User Id is invalid");

            // Act
            result = await _controller.UpdateSavedListing(1, new UpdateSavedListingRequest() { OldListingId = -1, NewListingId = 1});

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().Be("Either old or new Listing Id provided is invalid");

            // Act
            result = await _controller.UpdateSavedListing(1, new UpdateSavedListingRequest() { OldListingId = 1, NewListingId = -1});

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().Be("Either old or new Listing Id provided is invalid");
        }

        [Fact]
        public async Task UpdateSavedListing_ShouldReturnNotFound_WhenListingDoesNotExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.UpdateSavedListingAsync(1, 1, 1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateSavedListing(1, new UpdateSavedListingRequest() { OldListingId = 1, NewListingId = 1});

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
               .Which.Value.Should().Be("Saved listing not found");
        }

        [Fact]
        public async Task UpdateSavedListing_ShouldReturnNoContent_WhenListingExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.UpdateSavedListingAsync(1, 1, 1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateSavedListing(1, new UpdateSavedListingRequest() { OldListingId = 1, NewListingId = 1});

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteSavedListing_ShouldReturnNotFound_WhenListingDoesNotExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.DeleteSavedListingAsync(1, 1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteSavedListing(1, 1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("Saved listing not found");
        }

        [Fact]
        public async Task DeleteSavedListing_ShouldReturnNoContent_WhenListingExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.DeleteSavedListingAsync(1, 1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteSavedListing(1, 1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetSavedListingCount_ShouldReturnZeroCount_WhenListingDoesNotExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.GetSavedListingCountAsync(1))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.GetSavedListingCount(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                   .Which.Value.Should().Be(0);
        }

        [Fact]
        public async Task GetSavedListingCount_ShouldReturnCount_WhenListingExist()
        {
            // Arrange
            _mockSavedListingRepository.Setup(c => c.GetSavedListingCountAsync(1))
                .ReturnsAsync(2);

            // Act
            var result = await _controller.GetSavedListingCount(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                   .Which.Value.Should().Be(2);
        }
    }
}
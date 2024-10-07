using Listings.Domain.Models;
using Listings.Domain.Requests;
using Listings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Listings.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SavedListingController : ControllerBase
    {
        private readonly ISavedListingRepository _savedListingRepository;

        public SavedListingController(ISavedListingRepository savedListingRepository)
        {
            _savedListingRepository = savedListingRepository;
        }

        /// <summary>
        /// Get saved listings by user id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <response code="200">Returns Saved Listings</response>
        /// <response code="404">No Listings found for user</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SavedListing>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSavedListings(int userId)
        {
            var savedListings = await _savedListingRepository.GetSavedListingsByUserAsync(userId);

            if(savedListings == null || savedListings.Count() == 0)
            {
                return NotFound($"No Listings for user id {userId}");
            }

            return Ok(savedListings);
        }

        /// <summary>
        /// Create saved listing
        /// </summary>
        /// <param name="request">Request Body for SavedListing</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/savedlisting
        ///     {
        ///         "userId": 1,
        ///         "listingId": 1
        ///     }    
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly created saved listing</response>
        /// <response code="400">Invalid Request send by client </response>
        /// <response code="404">User Id or Listing Id does not exist</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SavedListing))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSavedListing([FromBody] CreateSavedListingRequest request)
        {
            if (request.UserId <= 0) 
            {
                return BadRequest("User Id is invalid");
            }

            if (request.ListingId <= 0) 
            {
                return BadRequest("Listing Id is invalid");
            }

            var createdListing = await _savedListingRepository.CreateSavedListingAsync(request.UserId, request.ListingId);

            if(createdListing == null)
            {
                return NotFound("User Id or Listing Id does not exist");
            }

            return Created("", createdListing);
        }

        /// <summary>
        /// Update saved listing
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="request">Request Body for Update Saved Listing</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/savedlisting/1
        ///     {
        ///         "oldlistingId": 1,
        ///         "newlistingId": 2
        ///     }
        ///    
        /// </remarks>
        /// <response code="204">Saved Listing updated successfully</response>
        /// <response code="400">Invalid Request send by client </response>
        /// <response code="404">Saved Listing Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSavedListing(int userId, [FromBody] UpdateSavedListingRequest request)
        {
            if (userId <= 0) 
            {
                return BadRequest("User Id is invalid");
            }

            if (request.OldListingId <= 0 || request.NewListingId <= 0) 
            {
                return BadRequest("Either old or new Listing Id provided is invalid");
            } 

            var result = await _savedListingRepository.UpdateSavedListingAsync(userId, request.OldListingId, request.NewListingId);
            if(!result)
            {
                return NotFound("Saved listing not found");
            }

            return NoContent();
        }

        /// <summary>
        /// Delete saved listing
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="listingId">Listing Id</param>
        /// <returns></returns>
        /// <response code="204">Saved Listing deleted successfully</response>
        /// <response code="404">Saved Listing Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{userId}/{listingId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSavedListing(int userId, int listingId)
        {
            var result = await _savedListingRepository.DeleteSavedListingAsync(userId, listingId);

            if (!result) 
            {
                return NotFound("Saved listing not found");
            }
            
            return NoContent();
        }

        /// <summary>
        /// Retrieve Number of Users for a listing
        /// </summary>
        /// <param name="listingId">Listing Id</param>
        /// <returns></returns>
        /// <response code="200">Returns the count of users for a listing</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("count/{listingId}")]
        public async Task<IActionResult> GetSavedListingCount(int listingId)
        {
            var count = await _savedListingRepository.GetSavedListingCountAsync(listingId);
            return Ok(count);
        }
    }
}
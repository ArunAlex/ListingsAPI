using Listings.Domain.Models;
using Listings.Domain.Requests;
using Listings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Listings.API.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        /// <response code="200">Returns User</response>
        /// <response code="404">User Not Found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null) 
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user">Request Body for user</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///         
        ///     POST /api/user
        ///     {
        ///         "username": "John",
        ///         "email": "test@emial.com",
        ///         "passwordhash": "password"     
        ///     }
        ///    
        /// </remarks>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">Invalid Request send by client </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Update user 
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="request">Request Body for user</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample requests:
        /// 
        ///     PUT /api/user/1
        ///     {
        ///       "username": "John",
        ///       "email": "test@email.com",
        ///       "passwordhash": "password"
        ///     }
        ///   
        ///     PUT /api/user/1
        ///     {
        ///       "username": "John",
        ///       "passwordhash": "password"
        ///     }
        ///     
        ///     PUT /api/user/1
        ///     {
        ///       "passwordhash": "password"
        ///     }
        ///     
        /// </remarks>
        /// <response code="204">User updated successfully</response>
        /// <response code="404">User Not Found</response>
        /// <response code="400">Invalid Request send by client </response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
                    
            var result = await _userRepository.UpdateUserAsync(id, request);
            if (!result) 
            {
                return NotFound("User not found");
            }

            return NoContent();
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        /// <response code="204">User deleted successfully</response>
        /// <response code="404">User Not Found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);

            if (!result) 
            {
                return NotFound("User not found");
            }

            return NoContent();
        }
    }
}

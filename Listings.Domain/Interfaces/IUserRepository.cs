using Listings.Domain.Models;
using Listings.Domain.Requests;

namespace Listings.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> CreateUserAsync(CreateUserRequest user);
        Task<bool> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
    }
}
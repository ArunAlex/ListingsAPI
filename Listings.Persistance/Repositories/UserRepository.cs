using Listings.Domain.Models;
using Listings.Domain.Requests;
using Listings.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Listings.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users
                        .Include(u => u.SavedListings)
                        .Where(u => u.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users =  await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                user.SavedListings = await _context.SavedListings
                    .Where(sl => sl.UserId == user.Id)
                    .ToListAsync();
            }

            return users;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) 
            {
                return false;
            }

            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;
            user.PasswordHash = request.PasswordHash ?? user.PasswordHash;

            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) 
            {
                return false;
            }

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
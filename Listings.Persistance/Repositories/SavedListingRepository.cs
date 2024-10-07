using Listings.Domain.Models;
using Listings.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Listings.Persistance.Repositories
{
    public class SavedListingRepository : ISavedListingRepository
    {
        private readonly AppDbContext _context;

        public SavedListingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SavedListing>> GetSavedListingsByUserAsync(int userId)
        {
            var savedListings = await _context.SavedListings
                .Include(sl => sl.Listing)
                .Include(sl => sl.User)
                .Where(sl => sl.UserId == userId)
                .ToListAsync();

            return savedListings;
        }

        public async Task<SavedListing> CreateSavedListingAsync(int userId, int listingId)
        {
            var doesUserExist = await _context.Users.AnyAsync(u => u.Id == userId);
            var doesListingExist = await _context.Listings.AnyAsync(l => l.Id == listingId);

            if(!doesUserExist || !doesListingExist)
            {
                return null;
            }

            var savedListing = await _context.SavedListings
                .FirstOrDefaultAsync(sl => sl.UserId == userId && sl.ListingId == listingId);

            if(savedListing == null)
            {
                savedListing = new SavedListing()
                {
                    UserId = userId,
                    ListingId = listingId
                };

                _context.SavedListings.Add(savedListing);
                await _context.SaveChangesAsync();  
            }

            return savedListing;
        }

        public async Task<bool> UpdateSavedListingAsync(int userId, int oldListingId, int newListingId)
        {
            var savedListing = await _context.SavedListings
                .FirstOrDefaultAsync(sl => sl.UserId == userId && sl.ListingId == oldListingId);
            
            if(savedListing == null)
            {
                return false;
            }

            //remove the old listing from the saved listings
            _context.SavedListings.Remove(savedListing);
            var isUpdated = await _context.SaveChangesAsync() > 0;

            if(isUpdated)
            {
                 //add the new listing to the saved listings
                var newSavedListing = await CreateSavedListingAsync(userId, newListingId);
                return newSavedListing != null;
            }

            return false;
        }

        public async Task<bool> DeleteSavedListingAsync(int userId, int listingId)
        {
            var savedListing = await _context.SavedListings
                .FirstOrDefaultAsync(sl => sl.UserId == userId && sl.ListingId == listingId);

            if (savedListing == null) 
            {
                return false;
            }

            _context.SavedListings.Remove(savedListing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetSavedListingCountAsync(int listingId)
        {
            // Return the number of users who have saved a listing with id listingId
            return await _context.SavedListings
                .Where(sl => sl.ListingId == listingId)
                .Select(u=>u.UserId)
                .Distinct()
                .CountAsync();
        }
    }
}
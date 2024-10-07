using Listings.Domain.Models;

namespace Listings.Domain.Interfaces
{
    public interface ISavedListingRepository
    {
        Task<IEnumerable<SavedListing>> GetSavedListingsByUserAsync(int userId);
        Task<SavedListing> CreateSavedListingAsync(int userId, int listingId);
        Task<bool> UpdateSavedListingAsync(int userId, int oldListingId, int newListingId);
        Task<bool> DeleteSavedListingAsync(int userId, int listingId);
        Task<int> GetSavedListingCountAsync(int listingId);
    }
}
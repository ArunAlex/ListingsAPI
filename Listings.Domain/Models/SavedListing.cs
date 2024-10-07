namespace Listings.Domain.Models
{
    public class SavedListing
    {
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int ListingId { get; set; }
        public Listing Listing { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}
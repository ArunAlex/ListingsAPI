namespace Listings.Domain.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public int Postcode { get; set; }

        // Navigation property for the users who saved this listing
        public ICollection<SavedListing> SavedByUsers { get; set; }
    }
}
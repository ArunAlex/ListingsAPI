namespace Listings.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property for the saved listings
        public ICollection<SavedListing> SavedListings { get; set; }
    }
}

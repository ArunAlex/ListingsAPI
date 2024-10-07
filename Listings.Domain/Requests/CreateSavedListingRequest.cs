namespace Listings.Domain.Requests;

public class CreateSavedListingRequest
{
    public int UserId { get; set; }
    public int ListingId { get; set; }
}

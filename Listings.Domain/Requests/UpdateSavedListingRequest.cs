namespace Listings.Domain.Requests;

public class UpdateSavedListingRequest
{
    public int OldListingId { get; set; }
    public int NewListingId { get; set; }
}

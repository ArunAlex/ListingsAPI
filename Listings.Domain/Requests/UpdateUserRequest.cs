using System.ComponentModel.DataAnnotations;

namespace Listings.Domain.Requests;

public class UpdateUserRequest
{       
    public string? Username { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Listings.Domain.Requests;

public class CreateUserRequest
{       
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
}